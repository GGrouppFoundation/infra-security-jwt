using System;
using System.IdentityModel.Tokens.Jwt;
using GGroupp.Infra;
using Microsoft.OpenApi.Models;

namespace Microsoft.AspNetCore.Builder;

partial class JwtValidationMiddleware
{
    public static TApplicationBuilder UseJwtValidation<TApplicationBuilder>(
        this TApplicationBuilder appBuilder,
        Func<IServiceProvider, ISecurityTokenValidateSupplier<JwtSecurityToken>> validationApiResolver,
        JwtValidationStatusCodes? jwtValidationStatusCodes = null)
        where TApplicationBuilder : class, IApplicationBuilder
    {
        _ = appBuilder ?? throw new ArgumentNullException(nameof(appBuilder));
        _ = validationApiResolver ?? throw new ArgumentNullException(nameof(validationApiResolver));

        if (jwtValidationStatusCodes is not null && jwtValidationStatusCodes.Value.NotSpecifiedHeaderValueStatusCode is null)
        {
            _ = appBuilder.UseWhen(IsAuthorizationHeaderSpecified, InnerConfigureMiddleware);
        }
        else
        {
            InnerConfigureMiddleware(appBuilder);
        }

        if (appBuilder is ISwaggerBuilder swaggerBuilder)
        {
            _ = swaggerBuilder.Use(InnerConfigureSwagger);
        }

        return appBuilder;

        void InnerConfigureMiddleware(IApplicationBuilder app)
            =>
            appBuilder.Use(next => context => context.InvokeJwtValidationAsync(next, validationApiResolver, jwtValidationStatusCodes));

        void InnerConfigureSwagger(OpenApiDocument openApiDocument)
            =>
            JwtValidationSwaggerConfigurator.Configure(openApiDocument, jwtValidationStatusCodes);
    }
}