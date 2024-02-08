using System;
using System.IdentityModel.Tokens.Jwt;
using PrimeFuncPack;

namespace GarageGroup.Infra;

public static class JwtSecurityTokenValidateApiDependency
{
    public static Dependency<ISecurityTokenValidateSupplier<JwtSecurityToken>> UseJwtSecurityTokenValidation(
        this Dependency<IIssuerSigningKeyApi, JwtValidationOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<ISecurityTokenValidateSupplier<JwtSecurityToken>>(CreateApi);

        static JwtSecurityTokenValidateApi CreateApi(IIssuerSigningKeyApi signingKeyApi, JwtValidationOption option)
        {
            ArgumentNullException.ThrowIfNull(signingKeyApi);
            ArgumentNullException.ThrowIfNull(option);

            return new(signingKeyApi, option);
        }
    }

    public static Dependency<ISecurityTokenValidateSupplier<JwtSecurityToken>> UseJwtSecurityTokenValidation(
        this Dependency<IIssuerSigningKeyApi> dependency, Func<IServiceProvider, JwtValidationOption> optionResolver)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        ArgumentNullException.ThrowIfNull(optionResolver);

        return dependency.Map<ISecurityTokenValidateSupplier<JwtSecurityToken>>(CreateApi);

        JwtSecurityTokenValidateApi CreateApi(IServiceProvider serviceProvider, IIssuerSigningKeyApi signingKeyApi)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(signingKeyApi);

            return new(signingKeyApi, optionResolver.Invoke(serviceProvider));
        }
    }
}