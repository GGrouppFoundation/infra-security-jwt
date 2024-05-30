using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;

namespace GarageGroup.Infra;

public static class JwtSecurityTokenValidatationApiDependency
{
    private const string DefaultSectionName = "Jwt";

    public static Dependency<ISecurityTokenValidatationApi<JwtSecurityToken>> UseJwtSecurityTokenValidation(
        this Dependency<IIssuerSigningKeyApi> dependency, string sectionName = DefaultSectionName)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<ISecurityTokenValidatationApi<JwtSecurityToken>>(CreateApi);

        JwtSecurityTokenValidatationApi CreateApi(IServiceProvider serviceProvider, IIssuerSigningKeyApi signingKeyApi)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(signingKeyApi);

            var section = serviceProvider.GetServiceOrThrow<IConfiguration>().GetRequiredSection(sectionName ?? string.Empty);

            return new(
                signingKeyApi: signingKeyApi,
                option: new(
                    publicKeyBase64: section.GetPublicKeyBase64OrThrow(),
                    validateLifetime: section.GetValue("ValidatationLifetime", true)));
        }
    }

    public static Dependency<ISecurityTokenValidatationApi<JwtSecurityToken>> UseJwtSecurityTokenValidation(
        this Dependency<IIssuerSigningKeyApi, JwtValidationOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<ISecurityTokenValidatationApi<JwtSecurityToken>>(CreateApi);

        static JwtSecurityTokenValidatationApi CreateApi(IIssuerSigningKeyApi signingKeyApi, JwtValidationOption option)
        {
            ArgumentNullException.ThrowIfNull(signingKeyApi);
            ArgumentNullException.ThrowIfNull(option);

            return new(signingKeyApi, option);
        }
    }

    private static string GetPublicKeyBase64OrThrow(this IConfigurationSection section)
    {
        var publicKeyBase64 = section["PublicKeyBase64"];

        if (string.IsNullOrWhiteSpace(publicKeyBase64))
        {
            throw new InvalidOperationException("PublicKeyBase64 must be specified");
        }

        return publicKeyBase64;
    }
}