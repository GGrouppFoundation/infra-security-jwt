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
        var tokenResult = await context.GetAuthValue().FlatMap(context.GetBearerValue).FlatMapValueAsync(ValidateAsync);

        _ = await tokenResult.Map(context.OnSuccess).FoldValueAsync(NextAsync, context.OnFailureAsync);

        ValueTask<Optional<JwtSecurityToken>> ValidateAsync(string token)
            =>
            validationApiResolver.Invoke(context.RequestServices).ValidateTokenAsync(token, context.RequestAborted);

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

    private static ValueTask<Unit> OnFailureAsync(this HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return default;
    }

    private static Optional<string?> GetAuthValue(this HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("Authorization", out var value))
        {
            return Optional.Present<string?>(value);
        }

        context.GetLogger()?.LogError("Authorization header value must be specified");
        return default;
    }

    private static Optional<string> GetBearerValue(this HttpContext context, string? authHeaderValue)
    {
        if (string.IsNullOrWhiteSpace(authHeaderValue))
        {
            context.GetLogger()?.LogError("Authorization header value must be not empty or a white space value");
            return default;
        }

        var arr = authHeaderValue.Split(' ');
        if (arr.Length is not 2)
        {
            context.GetLogger()?.LogError("Authorization header value '{authHeaderValue}' is invalid", authHeaderValue);
            return default;
        }

        if (string.Equals(BearerSchemaName, arr[0], StringComparison.InvariantCultureIgnoreCase) is false)
        {
            context.GetLogger()?.LogError("Authorization token '{authHeaderValue}' is invalid: the schema must be Bearer", authHeaderValue);
            return default;
        }

        return Optional.Present(arr[1]);
    }

    private static ILogger? GetLogger(this HttpContext context)
        =>
        context.RequestServices.GetServiceOrAbsent<ILoggerFactory>().OrDefault()?.CreateLogger("JwtValidationMiddleware");
}