using System;
using System.IdentityModel.Tokens.Jwt;

namespace GGroupp.Infra;

internal sealed partial class JwtSecurityTokenValidateApi : ISecurityTokenValidateSupplier<JwtSecurityToken>
{
    public static JwtSecurityTokenValidateApi Create(IIssuerSigningKeyApi signingKeyApi, JwtValidationOption option)
    {
        _ = signingKeyApi ?? throw new ArgumentNullException(nameof(signingKeyApi));
        _ = option ?? throw new ArgumentNullException(nameof(option));

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