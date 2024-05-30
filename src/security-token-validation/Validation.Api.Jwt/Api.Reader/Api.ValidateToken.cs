using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class JwtSecurityTokenReaderApi
{
    public ValueTask<Result<JwtSecurityToken, Failure<Unit>>> ValidateTokenAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return new(
                result: new());
        }

        return new(
            result: ReadJwtTokenOrFailure(accessToken));
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