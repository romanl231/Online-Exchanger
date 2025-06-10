namespace Exchanger.API.Enums.TokenGenerationErrors
{
    public enum TokenErrorCode
    {
        MissingUserId,
        InvalidCredentials,
        ExpiredRefreshToken,
        UserNotFound,
        TokenCreationFailed,
        InvalidScopes,
        MissingClaims,
        UserLockedOut,
        UserNotActive,
        UnexpectedError
    }
}
