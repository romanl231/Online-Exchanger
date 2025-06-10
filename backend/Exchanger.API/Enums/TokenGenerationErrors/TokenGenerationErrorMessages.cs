namespace Exchanger.API.Enums.TokenGenerationErrors
{
    public static class TokenErrorMessages
    {
        public static Dictionary<TokenErrorCode, string> Errors = new()
        {
            [TokenErrorCode.MissingUserId] = "User ID is missing or null.",
            [TokenErrorCode.InvalidCredentials] = "Invalid username or password.",
            [TokenErrorCode.ExpiredRefreshToken] = "The refresh token has expired.",
            [TokenErrorCode.UserNotFound] = "User not found.",
            [TokenErrorCode.TokenCreationFailed] = "Token creation failed due to internal error.",
            [TokenErrorCode.InvalidScopes] = "Provided scopes are invalid or unauthorized.",
            [TokenErrorCode.MissingClaims] = "Required claims are missing for token generation.",
            [TokenErrorCode.UserLockedOut] = "The user account is locked.",
            [TokenErrorCode.UserNotActive] = "The user account is not active.",
            [TokenErrorCode.UnexpectedError] = "An unexpected error occurred during token generation."
        };
    }
}
