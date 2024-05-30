namespace GarageGroup.Infra;

public sealed record class JwtValidationOption
{
    public JwtValidationOption(string publicKeyBase64, bool validateLifetime = true)
    {
        PublicKeyBase64 = publicKeyBase64 ?? string.Empty;
        ValidateLifetime = validateLifetime;
    }

    public string PublicKeyBase64 { get; }

    public bool ValidateLifetime { get; }
}