using System.Security.Claims;

namespace GarageGroup.Infra;

public interface ISecurityTokenCreationApi
{
    SecurityTokenValue CreateToken(ClaimsIdentity claimsIdentity);
}