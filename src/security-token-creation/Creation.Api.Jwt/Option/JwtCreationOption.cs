namespace GarageGroup.Infra;

public sealed record class JwtCreationOption
{
    private const int DefaultTtlInSeconds = 3600;

    public JwtCreationOption(string privateKeyBase64, int? ttlInSeconds = DefaultTtlInSeconds)
    {
        PrivateKeyBase64 = privateKeyBase64 ?? string.Empty;
        TtlInSeconds = ttlInSeconds > 0 ? ttlInSeconds.Value : DefaultTtlInSeconds;
    }

    public string PrivateKeyBase64 { get; }

    public int TtlInSeconds { get; }
}