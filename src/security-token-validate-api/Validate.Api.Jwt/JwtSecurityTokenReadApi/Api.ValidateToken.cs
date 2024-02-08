using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class JwtSecurityTokenReadApi
{
    public ValueTask<Result<JwtSecurityToken, Failure<Unit>>> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(token))
        {
            return new(
                result: new JwtSecurityToken());
        }

        return new(
            result: ReadJwtTokenOrFailure(token));
    }

    private Result<JwtSecurityToken, Failure<Unit>> ReadJwtTokenOrFailure(string token)
    {
        try
        {
            return jwtSecurityTokenHandler.ReadJwtToken(token);
        }
        catch (Exception ex)
        {
            return ex.ToFailure($"An unxpected exception was thrown when trying to read jwt '{token}'");
        }
    }
}