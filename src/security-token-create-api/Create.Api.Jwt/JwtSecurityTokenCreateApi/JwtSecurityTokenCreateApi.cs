using System;

namespace GGroupp.Infra;

internal sealed partial class JwtSecurityTokenCreateApi : ISecurityTokenCreateSupplier
{
    public static JwtSecurityTokenCreateApi Create(ISigningCredentialsApi signingCredentialsApi, JwtSecurityTokenCreateOption option)
    {
        ArgumentNullException.ThrowIfNull(signingCredentialsApi);
        ArgumentNullException.ThrowIfNull(option);

        return new(signingCredentialsApi, option);
    }

    private readonly ISigningCredentialsApi signingCredentialsApi;

    private readonly JwtSecurityTokenCreateOption option;

    private JwtSecurityTokenCreateApi(ISigningCredentialsApi signingCredentialsApi, JwtSecurityTokenCreateOption option)
    {
        this.signingCredentialsApi = signingCredentialsApi;
        this.option = option;
    }
}