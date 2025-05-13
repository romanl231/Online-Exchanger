using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Enums.AuthErrors;

namespace Exchanger.API.Services.IServices
{
    public interface IUserService
    {
        Task<bool> CheckUserExistanceByEmailAsync(string email);
        Task<AuthResult> RegisterUserAsync(AuthDTO authDTO);
        Task<DisplayUserInfoDTO> GetUserInfoAsync(Guid userId);
        AuthErrorCode? ValidateUserPassword(string password);
        Task<AuthResult> UpdateUserInfoAsync(UpdateProfileDTO updateProfileDTO); 
        Task<AuthResult> CheckDoesPasswordsMatches(AuthDTO authDTO);
    }
}
