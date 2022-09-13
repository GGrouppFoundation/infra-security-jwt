using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using GGroupp.Infra;
using PrimeFuncPack;

namespace Microsoft.AspNetCore.Builder;

public static partial class JwtValidationMiddleware
{
    private const string BearerSchemaName = "Bearer";

    private static async Task InvokeJwtValidationAsync(
        this HttpContext context, RequestDelegate next,
        Func<IServiceProvider, ISecurityTokenValidateSupplier<JwtSecurityToken>> validationApiResolver)
    {
        var tokenResult = await context.GetAuthValue().Forward(context.GetBearerValue).ForwardValueAsync(ValidateAsync);

        _ = await tokenResult.MapSuccess(context.OnSuccess).FoldValueAsync(NextAsync, context.OnFailureAsync);

        async ValueTask<Result<JwtSecurityToken, JwtValidationFailureCode>> ValidateAsync(string token)
        {
            var validationApi = validationApiResolver.Invoke(context.RequestServices); 
            var result = await validationApi.ValidateTokenAsync(token, context.RequestAborted).ConfigureAwait(false);

            return result.Fold(ToSuccess, GetInvalidTokenFailureCode);
        }

        static Result<JwtSecurityToken, JwtValidationFailureCode> ToSuccess(JwtSecurityToken token)
            =>
            token;

        static Result<JwtSecurityToken, JwtValidationFailureCode> GetInvalidTokenFailureCode()
            =>
            JwtValidationFailureCode.InvalidToken;

        ValueTask<Unit> NextAsync(Unit _)
            =>
            new(Unit.InvokeAsync(next.Invoke, context));
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

    private static ValueTask<Unit> OnFailureAsync(this HttpContext context, JwtValidationFailureCode failureCode)
    {
        context.Response.StatusCode = failureCode switch
        {
            JwtValidationFailureCode.InvalidToken   => StatusCodes.Status401Unauthorized,
            _                                       => StatusCodes.Status403Forbidden
        };

        return default;
    }

    private static Result<string, JwtValidationFailureCode> GetAuthValue(this HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("Authorization", out var value))
        {
            return(string)value;
        }

        context.GetLogger()?.LogError("Authorization header value must be specified");
        return JwtValidationFailureCode.NotSpecifiedHeaderValue;
    }

    private static Result<string, JwtValidationFailureCode> GetBearerValue(this HttpContext context, string? authHeaderValue)
    {
        if (string.IsNullOrWhiteSpace(authHeaderValue))
        {
            context.GetLogger()?.LogError("Authorization header value must be not empty or a white space value");
            return JwtValidationFailureCode.NotSpecifiedHeaderValue;
        }

        var arr = authHeaderValue.Split(' ');
        if (arr.Length is not 2)
        {
            context.GetLogger()?.LogError("Authorization header value '{authHeaderValue}' is invalid", authHeaderValue);
            return JwtValidationFailureCode.InvalidTypeHeaderValue;
        }

        if (string.Equals(BearerSchemaName, arr[0], StringComparison.InvariantCultureIgnoreCase) is false)
        {
            context.GetLogger()?.LogError("Authorization token '{authHeaderValue}' is invalid: the schema must be Bearer", authHeaderValue);
            return JwtValidationFailureCode.InvalidTypeHeaderValue;
        }

        return arr[1];
    }

    private static ILogger? GetLogger(this HttpContext context)
        =>
        context.RequestServices.GetServiceOrAbsent<ILoggerFactory>().OrDefault()?.CreateLogger("JwtValidationMiddleware");
}