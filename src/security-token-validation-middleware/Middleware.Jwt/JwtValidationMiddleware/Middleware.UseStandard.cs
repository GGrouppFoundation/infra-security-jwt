using System;
using System.IdentityModel.Tokens.Jwt;
using GGroupp.Infra;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;

namespace Microsoft.AspNetCore.Builder;

partial class JwtValidationMiddleware
{
    public static TApplicationBuilder UseJwtValidation<TApplicationBuilder>(this TApplicationBuilder appBuilder)
        where TApplicationBuilder : class, IApplicationBuilder
    {
        _ = appBuilder ?? throw new ArgumentNullException(nameof(appBuilder));

        _ = appBuilder.Use(static next => context => context.InvokeJwtValidationAsync(next, GetStandardValidationApi));

        if (appBuilder is ISwaggerBuilder swaggerBuilder)
        {
            _ = swaggerBuilder.Use(JwtValidationSwaggerConfigurator.Configure);
        }

        return appBuilder;
    }

    private static ISecurityTokenValidateSupplier<JwtSecurityToken> GetStandardValidationApi(IServiceProvider serviceProvider)
        =>
        Dependency.Of<IIssuerSigningKeyApi>(new RsaSecurityKeyApi())
        .With(GetStandardJwtValidationOption)
        .UseJwtSecurityTokenValidation()
        .Resolve(serviceProvider);

    private static JwtValidationOption GetStandardJwtValidationOption(this IServiceProvider serviceProvider)
        =>
        new(
            pubicKeyBase64: serviceProvider.GetServiceOrThrow<IConfiguration>()["Jwt:PubicKeyBase64"]);
}