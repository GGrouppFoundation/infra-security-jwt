using System.IdentityModel.Tokens.Jwt;

namespace GarageGroup.Infra;

public sealed partial class JwtSecurityTokenReaderApi : ISecurityTokenValidatationApi<JwtSecurityToken>
{
    public static JwtSecurityTokenReaderApi Instance { get; }

    static JwtSecurityTokenReaderApi()
        =>
        Instance = new();

    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;

    private JwtSecurityTokenReaderApi()
        =>
        jwtSecurityTokenHandler = new();
}