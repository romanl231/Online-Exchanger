using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Entities;
using Exchanger.API.Enums.AuthErrors;
using Exchanger.API.Repositories.IRepositories;
using Exchanger.API.Services.IServices;
using BCrypt.Net;

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
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                return false;

            return true;
        }

        public async Task<AuthResult> RegisterUserAsync(AuthDTO authDTO)
        {
            if (authDTO == null) 
                throw new ArgumentNullException("Empty auth DTO received");

            if (await CheckUserExistanceByEmailAsync(authDTO.Email))
                return AuthResult.Fail(AuthErrorCode.EmailUsed);

            var passwordValidation = ValidateUserPassword(authDTO.Password);

            if (passwordValidation != null)
                return AuthResult.Fail(passwordValidation.Value);

            var user = MapAuthDtoToUser(authDTO);

            if (!await _userRepository.AddAsync(user))
                throw new InvalidOperationException("An error ocurred while saving user to database");
            
            return AuthResult.Success(user);
        }

        public User MapAuthDtoToUser(AuthDTO authDTO)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Email = authDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(authDTO.Password),
                Name = "None",
                Surname = "None",
                AvatarUrl = "None",
            };
        }

        public AuthErrorCode? ValidateUserPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Empty password");

            if (password.Length < 8)
                return AuthErrorCode.ShortPassword;

            if (!password.Any(char.IsDigit))
                return AuthErrorCode.PasswordMustContainNum;

            if(!password.Any(char.IsAsciiLetter))
                return AuthErrorCode.PasswordMustContainChar;

            const string specialChars = @"!@#$%^&*()_+-=[]{}|;':"",.<>?/`~";
            if (!password.Any(specialChars.Contains))
                return AuthErrorCode.PasswordMustContainSpecialChar;

            if (!password.Any(char.IsUpper))
                return AuthErrorCode.PasswordMustContainUppercase;

            if (!password.Any(char.IsLower))
                return AuthErrorCode.PasswordMustContainLowercase;

            return null;
        }

        public async Task<AuthResult> CheckDoesPasswordsMatches(AuthDTO authDTO)
        {
            if (authDTO == null)
                throw new ArgumentNullException("Empty auth DTO received");

            var user = await _userRepository.GetByEmailAsync(authDTO.Email);
            if (user == null)
                return AuthResult.Fail(AuthErrorCode.UserNotFound);

            if (!BCrypt.Net.BCrypt.Verify(authDTO.Password, user.PasswordHash))
                return AuthResult.Fail(AuthErrorCode.InvalidCredentials);

            return AuthResult.Success(user);
        }

        public async Task<AuthResult> UpdateUserInfoAsync(UpdateProfileDTO updateProfileDTO)
        {
            return AuthResult.Fail(AuthErrorCode.InvalidCredentials);
        }
    }
}
