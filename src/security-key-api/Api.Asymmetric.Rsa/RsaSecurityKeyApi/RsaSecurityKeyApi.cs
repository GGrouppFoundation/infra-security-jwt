using System.Security.Cryptography;

namespace GarageGroup.Infra;

public sealed partial class RsaSecurityKeyApi : IIssuerSigningKeyApi, ISigningCredentialsApi
{
    private static RSA CreateRsa() => RSA.Create();
}