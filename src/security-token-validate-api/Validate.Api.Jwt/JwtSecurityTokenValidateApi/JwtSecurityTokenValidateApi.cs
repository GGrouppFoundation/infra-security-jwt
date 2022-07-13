using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

internal sealed partial class JwtSecurityTokenValidateApi : ISecurityTokenValidateSupplier<JwtSecurityToken>
{
    public static JwtSecurityTokenValidateApi Create(IIssuerSigningKeyApi signingKeyApi, JwtValidationOption option, ILoggerFactory? loggerFactory)
    {
        _ = signingKeyApi ?? throw new ArgumentNullException(nameof(signingKeyApi));
        _ = option ?? throw new ArgumentNullException(nameof(option));

        return new(signingKeyApi, option, loggerFactory?.CreateLogger<JwtSecurityTokenValidateApi>());
    }

    private readonly IIssuerSigningKeyApi signingKeyApi;

    private readonly JwtValidationOption option;

    private readonly ILogger? logger;

    private JwtSecurityTokenValidateApi(IIssuerSigningKeyApi signingKeyApi, JwtValidationOption option, ILogger? logger)
    {
        this.signingKeyApi = signingKeyApi;
        this.option = option;
        this.logger = logger;
    }
}