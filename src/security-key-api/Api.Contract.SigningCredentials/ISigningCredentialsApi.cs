using System;
using Microsoft.IdentityModel.Tokens;

namespace GarageGroup.Infra;

public interface ISigningCredentialsApi
{
    SigningCredentials GetSigningCredentials(ReadOnlySpan<byte> key);
}