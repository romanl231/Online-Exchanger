namespace Exchanger.API.Enums.TokenGenerationErrors
{
    public class TokenResult
    {
        public bool IsSuccess { get; init; }
        public string? Token { get; init; } = string.Empty;
        public Guid? UserId { get; init; }
        public TokenErrorCode? ErrorCode { get; init; }

        public static TokenResult Success(string token) => new()
        {
            IsSuccess = true,
            Token = token,
        };

        public static TokenResult Success() => new()
        {
            IsSuccess = true,
        };

        public static TokenResult Success(Guid? userId) => new()
        {
            IsSuccess = true,
            UserId = userId,
        };

        public static TokenResult Fail(TokenErrorCode errorCode) => new()
        {
            IsSuccess = false,
            ErrorCode = errorCode,
        };
    }
}
