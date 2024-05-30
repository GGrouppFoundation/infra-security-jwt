using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace GarageGroup.Infra;

public interface ISecurityTokenValidatationApi<TSecurityToken>
    where TSecurityToken : SecurityToken
{
    ValueTask<Result<TSecurityToken, Failure<Unit>>> ValidateTokenAsync(string accessToken, CancellationToken cancellationToken = default);
}