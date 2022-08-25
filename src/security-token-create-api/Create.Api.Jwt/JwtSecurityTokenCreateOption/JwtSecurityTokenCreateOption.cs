using System;

namespace GGroupp.Infra;

public sealed record class JwtSecurityTokenCreateOption
{
    public JwtSecurityTokenCreateOption(TimeSpan ttl, string privateKeyBase64)
    {
        Ttl = ttl;
        PrivateKeyBase64 = privateKeyBase64 ?? string.Empty;
    }

    public TimeSpan Ttl { get; }

    public string PrivateKeyBase64 { get; }
}