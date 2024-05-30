using System;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;

namespace GarageGroup.Infra;

public static class JwtSecurityTokenCreationApiDependency
{
    private const string DefaultSectionName = "Jwt";

    public static Dependency<ISecurityTokenCreationApi> UseJwtSecurityTokenCreationApi(
        this Dependency<ISigningCredentialsApi> dependency, string sectionName = DefaultSectionName)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<ISecurityTokenCreationApi>(CreateApi);

        JwtSecurityTokenCreationApi CreateApi(IServiceProvider serviceProvider, ISigningCredentialsApi signingCredentialsApi)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(signingCredentialsApi);

            var section = serviceProvider.GetServiceOrThrow<IConfiguration>().GetRequiredSection(sectionName ?? string.Empty);

            return new(
                signingCredentialsApi: signingCredentialsApi,
                option: new(
                    privateKeyBase64: section.GetPrivateKeyBase64OrThrow(),
                    ttlInSeconds: section.GetValue<int?>("TtlInSeconds")));
        }
    }

    public static Dependency<ISecurityTokenCreationApi> UseJwtSecurityTokenCreationApi(
        this Dependency<ISigningCredentialsApi, JwtCreationOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<ISecurityTokenCreationApi>(CreateApi);

        static JwtSecurityTokenCreationApi CreateApi(ISigningCredentialsApi signingCredentialsApi, JwtCreationOption option)
        {
            ArgumentNullException.ThrowIfNull(signingCredentialsApi);
            ArgumentNullException.ThrowIfNull(option);

            return new(signingCredentialsApi, option);
        }
    }

    private static string GetPrivateKeyBase64OrThrow(this IConfigurationSection section)
    {
        var privateKeyBase64 = section["PrivateKeyBase64"];

        if (string.IsNullOrWhiteSpace(privateKeyBase64))
        {
            throw new InvalidOperationException("PrivateKeyBase64 must be specified");
        }

        return privateKeyBase64;
    }
}