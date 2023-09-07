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
        return dependency.Fold<ISecurityTokenValidateSupplier<JwtSecurityToken>>(JwtSecurityTokenValidateApi.Create);
    }

    public static Dependency<ISecurityTokenValidateSupplier<JwtSecurityToken>> UseJwtSecurityTokenValidation(
        this Dependency<IIssuerSigningKeyApi> dependency, Func<IServiceProvider, JwtValidationOption> optionResolver)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        ArgumentNullException.ThrowIfNull(optionResolver);

        return dependency.With(optionResolver).Fold<ISecurityTokenValidateSupplier<JwtSecurityToken>>(JwtSecurityTokenValidateApi.Create);
    }
}