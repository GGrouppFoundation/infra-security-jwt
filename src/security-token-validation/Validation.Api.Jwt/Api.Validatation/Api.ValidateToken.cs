using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace GarageGroup.Infra;

partial class JwtSecurityTokenValidatationApi
{
    public ValueTask<Result<JwtSecurityToken, Failure<Unit>>> ValidateTokenAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<JwtSecurityToken, Failure<Unit>>>(cancellationToken);
        }

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return new(Failure.Create("JWT is not specified"));
        }

        var key = Convert.FromBase64String(option.PublicKeyBase64);

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

        return InnerValidateTokenAsync(accessToken, validationParameters);

        static bool LifetimeValidator(DateTime? before, DateTime? expire, SecurityToken securityToken, TokenValidationParameters _)
            =>
            DateTime.UtcNow < before || DateTime.UtcNow < expire;

        static bool DefaultLifetimeValidator(DateTime? before, DateTime? expire, SecurityToken securityToken, TokenValidationParameters _)
            =>
            true;
    }

    private async ValueTask<Result<JwtSecurityToken, Failure<Unit>>> InnerValidateTokenAsync(
        string accessToken, TokenValidationParameters validationParameters)
    {
        try
        {
            var securityTokenResult = await jwtSecurityTokenHandler.ValidateTokenAsync(accessToken, validationParameters).ConfigureAwait(false);

            if (securityTokenResult.IsValid is false)
            {
                return Failure.Create("The security token is invalid");
            }

            return (JwtSecurityToken)securityTokenResult.SecurityToken;
        }
        catch (Exception ex)
        {
            return ex.ToFailure($"An unxpected exception was thrown when trying to validate jwt '{accessToken}'");
        }
    }
}