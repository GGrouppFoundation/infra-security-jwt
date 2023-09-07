using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using GarageGroup.Infra;
using PrimeFuncPack;

namespace Microsoft.AspNetCore.Builder;

public static partial class JwtValidationMiddleware
{
    private const string BearerSchemaName = "Bearer";

    private const string NotSpecifiedAuthorizationHeaderValue = "Authorization header value was not specified";

    private const string WhiteSpaceAuthorizationHeaderValue = "Authorization header value was an empty string or a white space value";

    private static bool IsAuthorizationHeaderSpecified(HttpContext context)
        =>
        context.GetAuthValue().IsSuccess;

    private static async Task InvokeJwtValidationAsync(
        this HttpContext context,
        RequestDelegate next,
        Func<IServiceProvider, ISecurityTokenValidateSupplier<JwtSecurityToken>> validationApiResolver,
        JwtValidationStatusCodes? jwtValidationStatusCodes)
    {
        var tokenResult = await context.GetAuthValue().Forward(context.GetBearerValue).ForwardValueAsync(ValidateAsync);
        _ = await tokenResult.Recover(InnerInvokeFailure, context.OnSuccess).FoldValueAsync(NextAsync, StopAsync);

        async ValueTask<Result<JwtSecurityToken, Failure<JwtValidationFailureCode>>> ValidateAsync(string token)
        {
            var validationApi = validationApiResolver.Invoke(context.RequestServices);
            var result = await validationApi.ValidateTokenAsync(token, context.RequestAborted).ConfigureAwait(false);

            return result.MapFailure(MapJwtValidationFailure);
        }

        static Failure<JwtValidationFailureCode> MapJwtValidationFailure(Failure<Unit> failure)
            =>
            new(JwtValidationFailureCode.InvalidToken, failure.FailureMessage);

        Result<Unit, Unit> InnerInvokeFailure(Failure<JwtValidationFailureCode> failure)
            =>
            context.InvokeFailure(failure, jwtValidationStatusCodes);

        ValueTask<Unit> NextAsync(Unit _)
            =>
            new(Unit.InvokeAsync(next.Invoke, context));

        static ValueTask<Unit> StopAsync(Unit failure)
            =>
            new(failure);
    }

    private static Unit OnSuccess(this HttpContext context, JwtSecurityToken jwtSecurityToken)
    {
        if (context.User?.Identity is ClaimsIdentity claimsIdentity)
        {
            claimsIdentity.AddClaims(jwtSecurityToken.Claims);
        }
        else
        {
            context.User = new(new ClaimsIdentity(jwtSecurityToken.Claims));
        }

        return default;
    }

    private static Result<Unit, Unit> InvokeFailure(
        this HttpContext context, Failure<JwtValidationFailureCode> failure, JwtValidationStatusCodes? statusCodes)
    {
        var statusCode = failure.FailureCode.GetStatusCode(statusCodes);

        if (statusCode is null)
        {
            if (string.IsNullOrEmpty(failure.FailureMessage) is false)
            {
                context.GetLogger()?.LogInformation("{jwtFailureMessage}", failure.FailureMessage);
            }

            return Result.Present(default(Unit));
        }

        context.Response.StatusCode = statusCode.Value;

        if (string.IsNullOrEmpty(failure.FailureMessage) is false)
        {
            context.GetLogger()?.LogError("{jwtErrorMessage}", failure.FailureMessage);
        }

        return Result.Absent<Unit>();
    }

    private static Result<string, Failure<JwtValidationFailureCode>> GetAuthValue(this HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("Authorization", out var value) is false)
        {
            return Failure.Create(JwtValidationFailureCode.NotSpecifiedHeaderValue, NotSpecifiedAuthorizationHeaderValue);
        }

        var authValue = value.ToString();
        if (string.IsNullOrWhiteSpace(authValue))
        {
            return Failure.Create(JwtValidationFailureCode.NotSpecifiedHeaderValue, WhiteSpaceAuthorizationHeaderValue);
        }

        return authValue;
    }

    private static Result<string, Failure<JwtValidationFailureCode>> GetBearerValue(this HttpContext context, string authHeaderValue)
    {
        var arr = authHeaderValue.Split(' ');
        if (arr.Length is not 2)
        {
            return Failure.Create(JwtValidationFailureCode.InvalidTypeHeaderValue, $"Authorization header value '{authHeaderValue}' is invalid");
        }

        if (string.Equals(BearerSchemaName, arr[0], StringComparison.InvariantCultureIgnoreCase) is false)
        {
            return Failure.Create(JwtValidationFailureCode.InvalidTypeHeaderValue, $"Authorization token '{authHeaderValue}' is invalid: the schema must be Bearer");
        }

        return arr[1];
    }

    private static int? GetStatusCode(this JwtValidationFailureCode failureCode, JwtValidationStatusCodes? statusCodes)
    {
        if (statusCodes is null)
        {
            return StatusCodes.Status401Unauthorized;
        }

        return failureCode switch
        {
            JwtValidationFailureCode.NotSpecifiedHeaderValue => statusCodes.Value.NotSpecifiedHeaderValueStatusCode,
            JwtValidationFailureCode.InvalidTypeHeaderValue => statusCodes.Value.InvalidTokenStatusCode,
            JwtValidationFailureCode.InvalidToken => statusCodes.Value.InvalidTokenStatusCode,
            _ => StatusCodes.Status401Unauthorized
        };
    }

    private static ILogger? GetLogger(this HttpContext context)
        =>
        context.RequestServices.GetServiceOrAbsent<ILoggerFactory>().OrDefault()?.CreateLogger("JwtValidationMiddleware");
}