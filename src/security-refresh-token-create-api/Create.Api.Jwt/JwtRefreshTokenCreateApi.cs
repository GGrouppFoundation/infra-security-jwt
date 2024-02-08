using System;
using System.Security.Cryptography;

namespace GarageGroup.Infra;

public sealed class JwtRefreshTokenCreateApi : IRefreshTokenCreateSupplier
{
    public string CreateRefreshToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var randomNumberByteArr = new byte[32];

        rng.GetBytes(randomNumberByteArr);
        return Convert.ToBase64String(randomNumberByteArr);
    }
}