using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace GarageGroup.Infra;

partial class RsaExtensions
{
    internal static RsaSecurityKey ToSecurityKey(this RSA rsa)
        =>
        new(rsa);
}