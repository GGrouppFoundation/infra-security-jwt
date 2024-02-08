using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace GarageGroup.Infra;

partial class JwtSecurityTokenCreateApi
{
    public string CreateToken(ClaimsIdentity claimsIdentity)
    {
        var key = Convert.FromBase64String(option.PrivateKeyBase64);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow + option.Ttl,
            SigningCredentials = signingCredentialsApi.GetSigningCredentials(key)
        };

        var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
        return jwtSecurityTokenHandler.WriteToken(token);
    }
}