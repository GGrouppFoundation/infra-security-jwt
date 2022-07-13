using System;
using System.IdentityModel.Tokens.Jwt;
using GGroupp.Infra;

namespace Microsoft.AspNetCore.Builder;

partial class JwtValidationMiddleware
{
    public static IApplicationBuilder UseJwtValidation(
        this IApplicationBuilder appBuilder, Func<IServiceProvider, ISecurityTokenValidateSupplier<JwtSecurityToken>> validationApiResolver)
    {
        _ = appBuilder ?? throw new ArgumentNullException(nameof(appBuilder));
        _ = validationApiResolver ?? throw new ArgumentNullException(nameof(validationApiResolver));

        return appBuilder.Use(next => context => context.InvokeJwtValidationAsync(next, validationApiResolver));
    }
}