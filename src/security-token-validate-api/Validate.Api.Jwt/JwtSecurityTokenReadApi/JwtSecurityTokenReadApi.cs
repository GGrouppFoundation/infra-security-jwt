using System.IdentityModel.Tokens.Jwt;

namespace GarageGroup.Infra;

public sealed partial class JwtSecurityTokenReadApi : ISecurityTokenValidateSupplier<JwtSecurityToken>
{
    public static JwtSecurityTokenReadApi Instance { get; }

    static JwtSecurityTokenReadApi()
        =>
        Instance = new();

    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;

    private JwtSecurityTokenReadApi()
        =>
        jwtSecurityTokenHandler = new();
}