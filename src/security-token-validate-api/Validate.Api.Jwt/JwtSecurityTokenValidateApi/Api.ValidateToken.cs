using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace GGroupp.Infra;

partial class JwtSecurityTokenValidateApi
{
    public ValueTask<Optional<JwtSecurityToken>> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(token))
        {
            logger?.LogError("JWT must be specified");
            return default;
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Optional<JwtSecurityToken>>(cancellationToken);
        }

        return InnerValidateTokenAsync(token);
    }

    private async ValueTask<Optional<JwtSecurityToken>> InnerValidateTokenAsync(string token)
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
            logger?.LogError(securityTokenResult.Exception, "The security token is invalid");
            return default;
        }

        var jwtSecurityToken = (JwtSecurityToken)securityTokenResult.SecurityToken;
        return Optional.Present(jwtSecurityToken);
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