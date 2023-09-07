using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace GarageGroup.Infra;

public interface ISecurityTokenValidateSupplier<TSecurityToken>
    where TSecurityToken : SecurityToken
{
    ValueTask<Result<TSecurityToken, Failure<Unit>>> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
}