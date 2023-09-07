using System;
using System.IdentityModel.Tokens.Jwt;

namespace GarageGroup.Infra;

internal sealed partial class JwtSecurityTokenValidateApi : ISecurityTokenValidateSupplier<JwtSecurityToken>
{
    public static JwtSecurityTokenValidateApi Create(IIssuerSigningKeyApi signingKeyApi, JwtValidationOption option)
    {
        ArgumentNullException.ThrowIfNull(signingKeyApi);
        ArgumentNullException.ThrowIfNull(option);

        return new(signingKeyApi, option);
    }

    private readonly IIssuerSigningKeyApi signingKeyApi;

    private readonly JwtValidationOption option;

    private JwtSecurityTokenValidateApi(IIssuerSigningKeyApi signingKeyApi, JwtValidationOption option)
    {
        this.signingKeyApi = signingKeyApi;
        this.option = option;
    }
}