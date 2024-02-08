using System.IdentityModel.Tokens.Jwt;

namespace GarageGroup.Infra;

internal sealed partial class JwtSecurityTokenValidateApi : ISecurityTokenValidateSupplier<JwtSecurityToken>
{
    private readonly IIssuerSigningKeyApi signingKeyApi;

    private readonly JwtValidationOption option;

    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

    internal JwtSecurityTokenValidateApi(IIssuerSigningKeyApi signingKeyApi, JwtValidationOption option)
    {
        this.signingKeyApi = signingKeyApi;
        this.option = option;
    }
}