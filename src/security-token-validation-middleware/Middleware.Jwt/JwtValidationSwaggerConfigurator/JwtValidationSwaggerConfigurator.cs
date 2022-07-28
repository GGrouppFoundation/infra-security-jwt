namespace GGroupp.Infra;

public sealed partial class JwtValidationSwaggerConfigurator : ISwaggerConfigurator
{
    private const string SecuritySchemeKey = "jwtAuthorization";

    private const string UnauthorizedCode = "401";

    private JwtValidationSwaggerConfigurator()
    {
    }
}