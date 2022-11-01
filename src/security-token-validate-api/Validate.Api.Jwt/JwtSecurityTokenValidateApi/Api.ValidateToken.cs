using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace GGroupp.Infra;

partial class JwtSecurityTokenValidateApi
{
    public ValueTask<Result<JwtSecurityToken, Failure<Unit>>> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<JwtSecurityToken, Failure<Unit>>>(cancellationToken);
        }

        if (string.IsNullOrEmpty(token))
        {
            return new(Failure.Create("JWT is not specified"));
        }

        return InnerValidateTokenAsync(token);
    }

    private async ValueTask<Result<JwtSecurityToken, Failure<Unit>>> InnerValidateTokenAsync(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Convert.FromBase64String(option.PubicKeyBase64);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKeyApi.GetIssuerSigningKey(key),
            ValidateIssuer = false,
            ValidateLifetime = true,
            LifetimeValidator = option.ValidateLifetime ? LifetimeValidator : DefaultLifetimeValidator,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        var securityTokenResult = await tokenHandler.ValidateTokenAsync(token, validationParameters).ConfigureAwait(false);

        if (securityTokenResult.IsValid is false)
        {
            return Failure.Create("The security token is invalid");
        }

        return (JwtSecurityToken)securityTokenResult.SecurityToken;
    }

    private static bool LifetimeValidator(
        DateTime? before, 
        DateTime? expire, 
        SecurityToken securityToken,
        TokenValidationParameters _)
        =>
        DateTime.UtcNow < before || DateTime.UtcNow < expire;

    private static bool DefaultLifetimeValidator(
        DateTime? before, 
        DateTime? expire, 
        SecurityToken securityToken,
        TokenValidationParameters _)
        => 
        true;
}