using System.IdentityModel.Tokens.Jwt;

namespace GarageGroup.Infra;

internal sealed partial class JwtSecurityTokenCreationApi : ISecurityTokenCreationApi
{
    private const string BearerTokenType = "Bearer";

    private readonly ISigningCredentialsApi signingCredentialsApi;

    private readonly JwtCreationOption option;

    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

    internal JwtSecurityTokenCreationApi(ISigningCredentialsApi signingCredentialsApi, JwtCreationOption option)
    {
        this.signingCredentialsApi = signingCredentialsApi;
        this.option = option;
    }
}