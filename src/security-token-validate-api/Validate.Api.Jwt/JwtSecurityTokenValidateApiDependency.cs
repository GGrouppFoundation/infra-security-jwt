using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;
using PrimeFuncPack;

namespace GGroupp.Infra;

public static class JwtSecurityTokenValidateApiDependency
{
    public static Dependency<ISecurityTokenValidateSupplier<JwtSecurityToken>> UseJwtSecurityTokenValidation(
        this Dependency<IIssuerSigningKeyApi, JwtValidationOption> dependency)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.With(ResolveLoggerFactory).InnerUseJwtSecurityTokenValidation();
    }

    public static Dependency<ISecurityTokenValidateSupplier<JwtSecurityToken>> UseJwtSecurityTokenValidation(
        this Dependency<IIssuerSigningKeyApi> dependency,
        Func<IServiceProvider, JwtValidationOption> optionResolver)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = optionResolver ?? throw new ArgumentNullException(nameof(optionResolver));

        return dependency.With(optionResolver).With(ResolveLoggerFactory).InnerUseJwtSecurityTokenValidation();
    }

    public static Dependency<ISecurityTokenValidateSupplier<JwtSecurityToken>> UseJwtSecurityTokenValidation(
        this Dependency<IIssuerSigningKeyApi> dependency,
        Func<IServiceProvider, JwtValidationOption> optionResolver,
        Func<IServiceProvider, ILoggerFactory> loggerFactoryResolver)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = optionResolver ?? throw new ArgumentNullException(nameof(optionResolver));
        _ = loggerFactoryResolver ?? throw new ArgumentNullException(nameof(loggerFactoryResolver));

        // Logger factory will never be null here
        return dependency.With(optionResolver).With(loggerFactoryResolver)!.InnerUseJwtSecurityTokenValidation();
    }

    private static Dependency<ISecurityTokenValidateSupplier<JwtSecurityToken>> InnerUseJwtSecurityTokenValidation(
        this Dependency<IIssuerSigningKeyApi, JwtValidationOption, ILoggerFactory?> dependency)
        =>
        dependency.Fold<ISecurityTokenValidateSupplier<JwtSecurityToken>>(JwtSecurityTokenValidateApi.Create);

    private static ILoggerFactory? ResolveLoggerFactory(IServiceProvider serviceProvider)
        =>
        serviceProvider.GetServiceOrAbsent<ILoggerFactory>().OrDefault();
}