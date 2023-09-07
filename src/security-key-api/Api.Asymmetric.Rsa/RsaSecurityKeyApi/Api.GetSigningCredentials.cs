using System;
using Microsoft.IdentityModel.Tokens;

namespace GarageGroup.Infra;

partial class RsaSecurityKeyApi
{
    public SigningCredentials GetSigningCredentials(ReadOnlySpan<byte> key)
        =>
        new(
            key: CreateRsa().ImportPrivateKey(key).ToSecurityKey(),
            algorithm: SecurityAlgorithms.RsaSha256);
}