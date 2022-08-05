using System;

namespace GGroupp.Infra;

public sealed partial class SecurityTokenGetApi : ISecurityTokenGetSupplier
{
    private const string BearerSchemaName = "Bearer";
    
    public SecurityTokenGetApi()
    {
        
    }
}