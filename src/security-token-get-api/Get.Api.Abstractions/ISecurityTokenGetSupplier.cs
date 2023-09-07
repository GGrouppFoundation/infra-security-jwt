using System;

namespace GarageGroup.Infra;

public interface ISecurityTokenGetSupplier
{
    Result<string, Failure<SecurityTokenGetFailureCode>> GetJwtTokenValue(string authorizationString);
}