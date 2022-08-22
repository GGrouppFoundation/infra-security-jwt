namespace GGroupp.Infra;

public sealed record class JwtValidationOption
{
    public JwtValidationOption(string pubicKeyBase64, bool validateLifetime = true)
    {
        PubicKeyBase64 = pubicKeyBase64;
        ValidateLifetime = validateLifetime;
    }

    public string PubicKeyBase64 { get; }
    
    public bool ValidateLifetime { get; }
}