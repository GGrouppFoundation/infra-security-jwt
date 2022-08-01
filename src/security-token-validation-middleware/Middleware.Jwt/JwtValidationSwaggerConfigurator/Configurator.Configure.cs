using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;

namespace GGroupp.Infra;

partial class JwtValidationSwaggerConfigurator
{
    internal static void Configure(OpenApiDocument openApiDocument)
    {
        if (openApiDocument is null)
        {
            return;
        }

        openApiDocument.Components ??= new();
        openApiDocument.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();

        openApiDocument.Components.SecuritySchemes[SecuritySchemeKey] = new()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT authorization header"
        };

        var referenceScurityScheme = new OpenApiSecurityScheme
        {
            Reference = new()
            {
                Type = ReferenceType.SecurityScheme,
                Id = SecuritySchemeKey
            }
        };

        var securityRequirement = new OpenApiSecurityRequirement
        {
            [referenceScurityScheme] = Array.Empty<string>()
        };

        if (openApiDocument.Paths?.Count is not > 0)
        {
            return;
        }

        foreach (var path in openApiDocument.Paths.Values.SelectMany(GetOperations))
        {
            path.Security ??= new List<OpenApiSecurityRequirement>();
            path.Security.Add(securityRequirement);

            path.Responses ??= new OpenApiResponses();
            if (path.Responses.ContainsKey(UnauthorizedCode))
            {
                continue;
            }

            path.Responses[UnauthorizedCode] = new()
            {
                Description = "Unauthorized"
            };
        }
    }

    private static IEnumerable<OpenApiOperation> GetOperations(OpenApiPathItem pathItem)
        =>
        pathItem.Operations?.Select(GetValue) ?? Enumerable.Empty<OpenApiOperation>();

    private static TValue GetValue<TKey, TValue>(KeyValuePair<TKey, TValue> pair)
        =>
        pair.Value;
}