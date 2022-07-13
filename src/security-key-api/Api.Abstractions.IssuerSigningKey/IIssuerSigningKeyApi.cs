using System;
using Microsoft.IdentityModel.Tokens;

namespace GGroupp.Infra;

public interface IIssuerSigningKeyApi
{
    SecurityKey GetIssuerSigningKey(ReadOnlySpan<byte> key);
}