using System;
using Microsoft.IdentityModel.Tokens;

namespace GarageGroup.Infra;

partial class RsaSecurityKeyApi
{
    public SecurityKey GetIssuerSigningKey(ReadOnlySpan<byte> key)
        =>
        CreateRsa().ImportPublicKey(key).ToSecurityKey();
}