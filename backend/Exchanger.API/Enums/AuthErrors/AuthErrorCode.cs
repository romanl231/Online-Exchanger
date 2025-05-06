namespace Exchanger.API.Enums.AuthErrors
{
    public enum AuthErrorCode
    {
        InvalidCredentials,
        UserNotFound,
        EmailNotVerified,
        AccountLocked,
        TokenExpired,
        TokenInvalid,
        EmailUsed,
        ShortPassword,
        PasswordMustContainNum,
        PasswordMustContainChar,
        PasswordMustContainSpecialChar,
        PasswordMustContainUppercase,
    }
}
