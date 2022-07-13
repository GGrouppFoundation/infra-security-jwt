using System;
using System.Security.Cryptography;

namespace GGroupp.Infra;

partial class RsaExtensions
{
    internal static RSA ImportPublicKey(this RSA rsa, ReadOnlySpan<byte> privateKey)
    {
        rsa.ImportRSAPublicKey(privateKey, out var _);
        return rsa;
    }
}