using System;
using System.IdentityModel.Tokens.Jwt;
using GGroupp.Infra;

namespace Microsoft.AspNetCore.Builder;

partial class JwtValidationMiddleware
{
    public static TApplicationBuilder UseJwtValidation<TApplicationBuilder>(
        this TApplicationBuilder appBuilder!!, Func<IServiceProvider, ISecurityTokenValidateSupplier<JwtSecurityToken>> validationApiResolver!!)
        where TApplicationBuilder : class, IApplicationBuilder
    {
        _ = appBuilder.Use(next => context => context.InvokeJwtValidationAsync(next, validationApiResolver));

        if (appBuilder is ISwaggerBuilder swaggerBuilder)
        {
            _ = swaggerBuilder.Use(JwtValidationSwaggerConfigurator.Configure);
        }

        return appBuilder;
    }
}