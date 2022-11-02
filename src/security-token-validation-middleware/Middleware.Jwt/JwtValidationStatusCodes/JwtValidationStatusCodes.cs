namespace GGroupp.Infra;

public readonly record struct JwtValidationStatusCodes
{
    public JwtValidationStatusCodes(int? notSpecifiedHeaderValueStatusCode, int? invalidTypeHeaderValueStatusCode, int? invalidTokenStatusCode)
    {
        NotSpecifiedHeaderValueStatusCode = notSpecifiedHeaderValueStatusCode;
        InvalidTypeHeaderValueStatusCode = invalidTypeHeaderValueStatusCode;
        InvalidTokenStatusCode = invalidTokenStatusCode;
    }

    public int? NotSpecifiedHeaderValueStatusCode { get; }

    public int? InvalidTypeHeaderValueStatusCode { get; }

    public int? InvalidTokenStatusCode { get; }
}