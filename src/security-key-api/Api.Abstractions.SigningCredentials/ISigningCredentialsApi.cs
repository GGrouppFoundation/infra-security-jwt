using System;
using Microsoft.IdentityModel.Tokens;

namespace GGroupp.Infra;

public interface ISigningCredentialsApi
{
    SigningCredentials GetSigningCredentials(ReadOnlySpan<byte> key);
}