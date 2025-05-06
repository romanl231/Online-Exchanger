namespace Exchanger.API.Enums.AuthErrors
{
    public static class AuthErrorMessages
    {
        public static readonly Dictionary<AuthErrorCode, string> Messages = new()
        {
            { AuthErrorCode.InvalidCredentials, "Invalid email or password." },
            { AuthErrorCode.UserNotFound, "User not found." },
            { AuthErrorCode.EmailNotVerified, "Please verify your email address." },
            { AuthErrorCode.AccountLocked, "Your account has been locked." },
            { AuthErrorCode.TokenExpired, "The token has expired." },
            { AuthErrorCode.TokenInvalid, "Invalid token." },
            { AuthErrorCode.EmailUsed, "Email is taken already" },
            { AuthErrorCode.ShortPassword, "Password length must be at least 8 chars" },
            { AuthErrorCode.PasswordMustContainChar, "Password must contain at least 1 letter" },
            { AuthErrorCode.PasswordMustContainNum, "Password must contain at least 1 number" },
            { AuthErrorCode.PasswordMustContainSpecialChar, "Password must contain at least 1 special char" },
            { AuthErrorCode.PasswordMustContainUppercase, "Password must contain at least 1 uppercase letter" },
            { AuthErrorCode.PasswordMustContainLowercase, "Password must contain at least 1 lowercase letter" },
        };
    }
}
