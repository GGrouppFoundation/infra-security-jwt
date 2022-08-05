using System;

namespace GGroupp.Infra;

partial class SecurityTokenGetApi
{
    public Result<string, Failure<SecurityTokenGetFailureCode>> GetJwtTokenValue(string authorizationString)
    {
        if (string.IsNullOrWhiteSpace(authorizationString))
        {
            return Failure.Create(
                SecurityTokenGetFailureCode.EmptyAuthorizationString, 
                "Authorization header value must be not empty or a white space value");
        }

        var arr = authorizationString.Split(' ');
        if (arr.Length is not 2)
        {
            return Failure.Create(
                SecurityTokenGetFailureCode.InvalidAuthorizationString, 
                $"Authorization header value '{authorizationString}' is invalid");
        }

        if (string.Equals(BearerSchemaName, arr[0], StringComparison.InvariantCultureIgnoreCase) is false)
        {
            return Failure.Create(
                SecurityTokenGetFailureCode.InvalidAuthorizationSchema, 
                $"Authorization token '{authorizationString}' is invalid: the schema must be Bearer");
        }

        return arr[1];
    }
}