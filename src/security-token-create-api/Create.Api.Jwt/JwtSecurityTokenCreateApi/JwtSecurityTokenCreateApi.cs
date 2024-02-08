using System;
using System.IdentityModel.Tokens.Jwt;

namespace GarageGroup.Infra;

internal sealed partial class JwtSecurityTokenCreateApi : ISecurityTokenCreateSupplier
{
    public static JwtSecurityTokenCreateApi Create(ISigningCredentialsApi signingCredentialsApi, JwtSecurityTokenCreateOption option)
    {
        ArgumentNullException.ThrowIfNull(signingCredentialsApi);
        ArgumentNullException.ThrowIfNull(option);

        return new(signingCredentialsApi, option);
    }

    private const string BearerTokenType = "Bearer";

    private readonly ISigningCredentialsApi signingCredentialsApi;

    private readonly JwtSecurityTokenCreateOption option;

    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

    private JwtSecurityTokenCreateApi(ISigningCredentialsApi signingCredentialsApi, JwtSecurityTokenCreateOption option)
    {
        this.signingCredentialsApi = signingCredentialsApi;
        this.option = option;
    }
}