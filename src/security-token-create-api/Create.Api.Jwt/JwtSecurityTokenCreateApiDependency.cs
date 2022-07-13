using System;
using PrimeFuncPack;

namespace GGroupp.Infra;

public static class JwtSecurityTokenCreateApiDependency
{
    public static Dependency<ISecurityTokenCreateSupplier> UseSecurityTokenCreateApi(
        this Dependency<ISigningCredentialsApi, JwtSecurityTokenCreateOption> dependency)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Fold<ISecurityTokenCreateSupplier>(JwtSecurityTokenCreateApi.Create);
    }

    public static Dependency<ISecurityTokenCreateSupplier> UseSecurityTokenCreateApi(
        this Dependency<ISigningCredentialsApi> dependency, Func<IServiceProvider, JwtSecurityTokenCreateOption> optionResolver)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = optionResolver ?? throw new ArgumentNullException(nameof(optionResolver));

        return dependency.With(optionResolver).Fold<ISecurityTokenCreateSupplier>(JwtSecurityTokenCreateApi.Create);
    }
}