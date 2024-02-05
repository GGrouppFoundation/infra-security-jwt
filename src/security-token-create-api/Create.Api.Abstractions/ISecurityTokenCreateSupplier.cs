using System.Security.Claims;

namespace GarageGroup.Infra;

public interface ISecurityTokenCreateSupplier
{
    string CreateToken(ClaimsIdentity claimsIdentity);

    SecurityTokenValue CreateTokenValue(ClaimsIdentity claimsIdentity);
}