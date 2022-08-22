using System;
using Microsoft.IdentityModel.Tokens;

namespace GGroupp.Infra;

public sealed record class JwtValidationOption
{
    public JwtValidationOption(string pubicKeyBase64)
    {
        PubicKeyBase64 = pubicKeyBase64 ?? string.Empty;
        LifetimeValidator = 
            static (before, expires, _, _) => 
                DateTime.UtcNow < before || DateTime.UtcNow < expires;
    }

    public JwtValidationOption(string pubicKeyBase64, LifetimeValidator lifetimeValidator)
    {
        PubicKeyBase64 = pubicKeyBase64;
        LifetimeValidator = lifetimeValidator;
    }

    public string PubicKeyBase64 { get; }
    
    public LifetimeValidator LifetimeValidator { get; }
}