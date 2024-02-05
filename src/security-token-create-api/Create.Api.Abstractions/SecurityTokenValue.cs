using System;

namespace GarageGroup.Infra;

public sealed record class SecurityTokenValue
{
    public SecurityTokenValue(string tokenType, string token, TimeSpan ttl)
    {
        TokenType = tokenType ?? string.Empty;
        Token = token ?? string.Empty;
        Ttl = ttl;
    }

    public string TokenType { get; }

    public string Token { get; }

    public TimeSpan Ttl { get; }
}