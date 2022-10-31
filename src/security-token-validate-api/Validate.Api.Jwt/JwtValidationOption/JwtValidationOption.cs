namespace GGroupp.Infra;

public sealed record class JwtValidationOption
{
    public JwtValidationOption(string pubicKeyBase64)
        =>
        PubicKeyBase64 = pubicKeyBase64 ?? string.Empty;

    public string PubicKeyBase64 { get; }
}