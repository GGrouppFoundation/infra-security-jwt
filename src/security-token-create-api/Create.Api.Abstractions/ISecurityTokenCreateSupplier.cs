using System.Security.Claims;

namespace GGroupp.Infra;

public interface ISecurityTokenCreateSupplier
{
    string CreateToken(ClaimsIdentity claimsIdentity);
}