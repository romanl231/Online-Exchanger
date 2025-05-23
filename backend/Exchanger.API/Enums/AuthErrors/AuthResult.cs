using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Entities;

namespace Exchanger.API.Enums.AuthErrors
{
    public class AuthResult
    {
        public bool IsSuccess { get; init; }
        public DisplayUserInfoDTO? UserInfo { get; init; }
        public User? User { get; init; }
        public AuthErrorCode? ErrorCode { get; init; }
        public OnCreationSessionDTO? OnCreationSession { get; set; }

        public static AuthResult Success(DisplayUserInfoDTO userInfo) => new() { IsSuccess = true, UserInfo = userInfo };
        public static AuthResult Success(User user) => new() { IsSuccess = true, User = user };
        public static AuthResult Success() => new() { IsSuccess = true};

        public static AuthResult Fail(AuthErrorCode error) => new() { IsSuccess = false, ErrorCode = error };

        public AuthResult WithSession(OnCreationSessionDTO session)
        {
            OnCreationSession = session;
            return this;
        }

    }
}
