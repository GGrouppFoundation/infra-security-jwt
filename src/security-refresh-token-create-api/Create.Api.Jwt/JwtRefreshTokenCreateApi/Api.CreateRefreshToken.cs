using System;
using System.Security.Cryptography;

namespace GGroupp.Infra;

partial class JwtRefreshTokenCreateApi
{
    public string CreateRefreshToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var randomNumberByteArr = new byte[32];

        rng.GetBytes(randomNumberByteArr);
        return Convert.ToBase64String(randomNumberByteArr);
    }
}