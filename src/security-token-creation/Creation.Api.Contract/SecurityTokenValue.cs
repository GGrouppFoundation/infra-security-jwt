namespace GarageGroup.Infra;

public sealed record class SecurityTokenValue
{
    public SecurityTokenValue(string tokenType, string accessToken, int expiresInSeconds, string refreshToken)
    {
        TokenType = tokenType ?? string.Empty;
        AccessToken = accessToken ?? string.Empty;
        ExpiresInSeconds = expiresInSeconds;
        RefreshToken = refreshToken ?? string.Empty;
    }

    public string TokenType { get; }

    public string AccessToken { get; }

    public int ExpiresInSeconds { get; }

    public string RefreshToken { get; }
}