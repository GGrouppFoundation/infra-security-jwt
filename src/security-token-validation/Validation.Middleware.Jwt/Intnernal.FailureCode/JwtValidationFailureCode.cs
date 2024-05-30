namespace GarageGroup.Infra;

internal enum JwtValidationFailureCode
{
    NotSpecifiedHeaderValue,

    InvalidTypeHeaderValue,

    InvalidToken
}