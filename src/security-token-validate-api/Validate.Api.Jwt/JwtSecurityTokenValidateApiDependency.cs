using System;
using System.IdentityModel.Tokens.Jwt;
using PrimeFuncPack;

namespace GGroupp.Infra;

public static class JwtSecurityTokenValidateApiDependency
{
    public static Dependency<ISecurityTokenValidateSupplier<JwtSecurityToken>> UseJwtSecurityTokenValidation(
        this Dependency<IIssuerSigningKeyApi, JwtValidationOption> dependency)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Fold<ISecurityTokenValidateSupplier<JwtSecurityToken>>(JwtSecurityTokenValidateApi.Create);
    }

    public static Dependency<ISecurityTokenValidateSupplier<JwtSecurityToken>> UseJwtSecurityTokenValidation(
        this Dependency<IIssuerSigningKeyApi> dependency, Func<IServiceProvider, JwtValidationOption> optionResolver)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = optionResolver ?? throw new ArgumentNullException(nameof(optionResolver));

        return dependency.With(optionResolver).Fold<ISecurityTokenValidateSupplier<JwtSecurityToken>>(JwtSecurityTokenValidateApi.Create);
    }
}