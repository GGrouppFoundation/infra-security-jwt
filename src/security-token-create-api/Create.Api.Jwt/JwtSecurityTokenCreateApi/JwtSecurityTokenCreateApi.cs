using System;

namespace GGroupp.Infra;

internal sealed partial class JwtSecurityTokenCreateApi : ISecurityTokenCreateSupplier
{
    public static JwtSecurityTokenCreateApi Create(ISigningCredentialsApi signingCredentialsApi, JwtSecurityTokenCreateOption option)
    {
        _ = signingCredentialsApi ?? throw new ArgumentNullException(nameof(signingCredentialsApi));
        _ = option ?? throw new ArgumentNullException(nameof(option));

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