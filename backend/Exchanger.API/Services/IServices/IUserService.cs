using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Enums.AuthErrors;

namespace Exchanger.API.Services.IServices
{
    public interface IUserService
    {
        Task<bool> CheckUserExistanceByEmailAsync(string email);
        Task<AuthResult> RegisterUserAsync(AuthDTO authDTO);
        Task<AuthResult> ValidateUserPasswordAsync(AuthDTO authDTO);
        Task<AuthResult> UpdateUserInfoAsync(UpdateProfileDTO updateProfileDTO);
    }
}
