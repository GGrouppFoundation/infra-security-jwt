using System;
using PrimeFuncPack;

namespace GGroupp.Infra;

public static class JwtSecurityTokenCreateApiDependency
{
    public static Dependency<ISecurityTokenCreateSupplier> UseSecurityTokenCreateApi(
        this Dependency<ISigningCredentialsApi, JwtSecurityTokenCreateOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<ISecurityTokenCreateSupplier>(JwtSecurityTokenCreateApi.Create);
    }

    public static Dependency<ISecurityTokenCreateSupplier> UseSecurityTokenCreateApi(
        this Dependency<ISigningCredentialsApi> dependency, Func<IServiceProvider, JwtSecurityTokenCreateOption> optionResolver)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        ArgumentNullException.ThrowIfNull(optionResolver);

        return dependency.With(optionResolver).Fold<ISecurityTokenCreateSupplier>(JwtSecurityTokenCreateApi.Create);
    }
}