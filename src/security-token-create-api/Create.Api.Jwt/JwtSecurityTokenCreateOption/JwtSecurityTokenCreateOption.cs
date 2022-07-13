namespace GGroupp.Infra;

public sealed record class JwtSecurityTokenCreateOption
{
    public JwtSecurityTokenCreateOption(int expiresDays, string privateKeyBase64)
    {
        ExpiresDays = expiresDays;
        PrivateKeyBase64 = privateKeyBase64 ?? string.Empty;
    }

    public int ExpiresDays { get; }

    public string PrivateKeyBase64 { get; }
}