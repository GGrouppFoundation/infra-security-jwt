using System;

namespace GGroupp.Infra;

public interface ISecurityTokenGetSupplier
{
    Result<string, Failure<SecurityTokenGetFailureCode>> GetJwtTokenValue(string authorizationString);
}