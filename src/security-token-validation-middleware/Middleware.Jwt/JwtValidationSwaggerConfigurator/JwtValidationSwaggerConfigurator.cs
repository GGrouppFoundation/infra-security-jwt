using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace GGroupp.Infra;

internal static partial class JwtValidationSwaggerConfigurator
{
    private const string SecuritySchemeKey = "jwtAuthorization";

    private static IReadOnlyCollection<KeyValuePair<string, string?>> GetCodeDescriptions(
        JwtValidationStatusCodes? jwtValidationStatusCodes)
    {
        if (jwtValidationStatusCodes is null)
        {
            return new KeyValuePair<string, string?>[] { new("401", "Unauthorized") };
        }

        return InnerGetCodes(jwtValidationStatusCodes.Value).Distinct().Select(GetCodeDescription).ToArray();

        static IEnumerable<int> InnerGetCodes(JwtValidationStatusCodes statusCodes)
        {
            if (statusCodes.NotSpecifiedHeaderValueStatusCode is not null)
            {
                yield return statusCodes.NotSpecifiedHeaderValueStatusCode.Value;
            }

            if (statusCodes.InvalidTypeHeaderValueStatusCode is not null)
            {
                yield return statusCodes.InvalidTypeHeaderValueStatusCode.Value;
            }

            if (statusCodes.InvalidTokenStatusCode is not null)
            {
                yield return statusCodes.InvalidTokenStatusCode.Value;
            }
        }

        static KeyValuePair<string, string?> GetCodeDescription(int code)
            =>
            new(code.ToString(), code.GetDescription());
    }

    private static string? GetDescription(this int code)
        =>
        Enum.GetName(typeof(HttpStatusCode), code);
}