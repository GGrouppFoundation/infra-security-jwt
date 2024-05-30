using System;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace GarageGroup.Infra;

partial class JwtSecurityTokenCreationApi
{
    public SecurityTokenValue CreateToken(ClaimsIdentity claimsIdentity)
    {
        var key = Convert.FromBase64String(option.PrivateKeyBase64);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddSeconds(option.TtlInSeconds),
            SigningCredentials = signingCredentialsApi.GetSigningCredentials(key)
        };

        var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);

        return new(
            tokenType: BearerTokenType,
            accessToken: jwtSecurityTokenHandler.WriteToken(token),
            expiresInSeconds: option.TtlInSeconds,
            refreshToken: GenerateRefreshToken());
    }

    private static string GenerateRefreshToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var randomNumberByteArr = new byte[32];

        rng.GetBytes(randomNumberByteArr);
        return Convert.ToBase64String(randomNumberByteArr);
    }
}