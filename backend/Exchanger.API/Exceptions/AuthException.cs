using Exchanger.API.Enums.AuthErrors;

namespace Exchanger.API.Exceptions
{
    public class AuthException : Exception
    {
        public AuthErrorCode Code { get; }

        public AuthException(AuthErrorCode code)
            : base(AuthErrorMessages
                  .Messages
                  .TryGetValue(code, out var msg) ? msg : "Unknown error")
        {
            Code = code;
        }
    }
}
