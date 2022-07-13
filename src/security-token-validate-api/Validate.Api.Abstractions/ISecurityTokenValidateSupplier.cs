using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace GGroupp.Infra;

public interface ISecurityTokenValidateSupplier<TSecurityToken>
    where TSecurityToken : SecurityToken
{
    ValueTask<Optional<TSecurityToken>> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
}