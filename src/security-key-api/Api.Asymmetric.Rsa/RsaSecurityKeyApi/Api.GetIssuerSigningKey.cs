using System;
using Microsoft.IdentityModel.Tokens;

namespace GGroupp.Infra;

partial class RsaSecurityKeyApi
{
    public SecurityKey GetIssuerSigningKey(ReadOnlySpan<byte> key)
        =>
        CreateRsa().ImportPublicKey(key).ToSecurityKey();
}