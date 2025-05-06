using Exchanger.API.Entities;

namespace Exchanger.API.Enums.AuthErrors
{
    public class AuthResult
    {
        public bool IsSuccess { get; init; }
        public User? User { get; init; }
        public AuthErrorCode? ErrorCode { get; init; }

        public static AuthResult Success(User user) => new() { IsSuccess = true, User = user };

        public static AuthResult Fail(AuthErrorCode error) => new() { IsSuccess = false, ErrorCode = error };

    }
}
