using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Enums.AuthErrors;
using Exchanger.API.Repositories.IRepositories;
using Exchanger.API.Services.IServices;

namespace Exchanger.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public async Task<bool> CheckUserExistanceByEmailAsync(string email)
        {
            return true;
        }

        public async Task<AuthResult> RegisterUserAsync(AuthDTO authDTO)
        {
            return AuthResult.Fail(AuthErrorCode.InvalidCredentials);
        }

        public async Task<AuthResult> ValidateUserPasswordAsync(AuthDTO authDTO)
        {
            return AuthResult.Fail(AuthErrorCode.InvalidCredentials);
        }

        public async Task<AuthResult> UpdateUserInfoAsync(UpdateProfileDTO updateProfileDTO)
        {
            return AuthResult.Fail(AuthErrorCode.InvalidCredentials);
        }
    }
}
