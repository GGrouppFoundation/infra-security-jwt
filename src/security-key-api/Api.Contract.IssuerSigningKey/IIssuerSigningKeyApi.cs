using System;
using Microsoft.IdentityModel.Tokens;

namespace GarageGroup.Infra;

public interface IIssuerSigningKeyApi
{
    SecurityKey GetIssuerSigningKey(ReadOnlySpan<byte> key);
}