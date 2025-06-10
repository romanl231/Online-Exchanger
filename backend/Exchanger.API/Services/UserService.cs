using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Entities;
using Exchanger.API.Enums.AuthErrors;
using Exchanger.API.Repositories.IRepositories;
using Exchanger.API.Services.IServices;
using BCrypt.Net;
using Exchanger.API.Enums.UploadToCloudErrors;
using CloudinaryDotNet.Actions;

namespace Exchanger.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ITokenService _tokenService;
        private readonly IEmailSenderService _emailSenderService;

        public UserService(
            IUserRepository userRepository, 
            ICloudinaryService cloudinaryService,
            ITokenService tokenService,
            IEmailSenderService emailSenderService) 
        {
            _userRepository = userRepository;
            _cloudinaryService = cloudinaryService;
            _tokenService = tokenService;
            _emailSenderService = emailSenderService;
        }

        public async Task<bool> CheckUserExistanceByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null; ;
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

            var result = MapUserToDisplayUserInfoDTO(user);
            return AuthResult.Success(result);
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

        public async Task<DisplayUserInfoDTO> GetUserInfoAsync(Guid userId)
        {
            var userEntity = await _userRepository.GetByIdAsync(userId);
            if (userEntity == null)
                throw new ArgumentNullException("Wrong user ID");

            return MapUserToDisplayUserInfoDTO(userEntity);
        }

        public DisplayUserInfoDTO MapUserToDisplayUserInfoDTO(User user)
        {
            return new DisplayUserInfoDTO
            {
                Email = user.Email,
                FirstName = user.Name,
                Surname = user.Surname,
                AvatarUrl = user.AvatarUrl,
            };
        }

        public async Task<AuthResult> UpdateUserInfoAsync(UpdateProfileDTO updateProfileDTO, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentNullException("Wrong user ID");

            UpdateUserEntity(updateProfileDTO, user);
            if (!await _userRepository.UpdateAsync(user))
                return AuthResult.Fail(AuthErrorCode.InvalidCredentials);

            return AuthResult.Success();
        }

        public async Task<CloudResult> UploadAvatarAsync(IFormFile image, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentNullException("Wrong userId");

            var cloudResponse = await _cloudinaryService.UploadAvatarToCloudAsync(image, userId);

            if (cloudResponse == null || !cloudResponse.IsSuccess)
                return cloudResponse ?? CloudResult.Fail(CloudErrorCode.UnexpectedCloudError);

            UpdateUserEntity(cloudResponse.UploadResult.SecureUrl.AbsoluteUri, user);

            if (!await _userRepository.UpdateAsync(user))
                return CloudResult.Fail(CloudErrorCode.UnexpectedCloudError);

            return CloudResult.Success();
        }

        public void UpdateUserEntity(string avatarUrl, User userEntity)
        {
            userEntity.AvatarUrl = avatarUrl;
        }

        public void UpdateUserEntity(UpdateProfileDTO userDTO, User userEntity)
        {
            userEntity.Name = userDTO.Name;
            userEntity.Surname = userDTO.Surname;
        }

        public async Task<AuthResult> InitiateEmailConfirmationAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new ArgumentNullException("Wrong user ID");

            var tokenResult = await _tokenService.GenerateEmailConfirmationTokenAsync(userId);

            if (!tokenResult.IsSuccess || string.IsNullOrEmpty(tokenResult.Token))
                throw new InvalidOperationException(tokenResult.ErrorCode.ToString());
            

            var emailSenderResult = await _emailSenderService.SendVerificationEmailAsync(user.Email, tokenResult.Token);

            if (!emailSenderResult.IsSuccess)
                throw new InvalidOperationException(emailSenderResult.ErrorCode.ToString());

            user.EmailVerificationCode = tokenResult.Token;
            var userResult = await _userRepository.UpdateAsync(user);

            if (!userResult)
                throw new InvalidOperationException("An unkown error occured while updating of user entity");

            return AuthResult.Success();
        }

        public async Task<AuthResult> ConfirmUserEmailAsync(string token)
        {
            var validationResult = await _tokenService
                .ValidateEmailConfirmationTokenAsync(token);

            if (!validationResult.IsSuccess || validationResult.UserId == Guid.Empty)
                return AuthResult.Fail(AuthErrorCode.TokenExpired);

            var user = await _userRepository.GetByIdAsync(validationResult.UserId.Value);
            
            if (user == null)
                return AuthResult.Fail(AuthErrorCode.UserNotFound);

            user.IsEmailVerified = true;
            var result = await _userRepository.UpdateAsync(user);

            if (!result)
                return AuthResult.Fail(AuthErrorCode.UserNotFound);

            return AuthResult.Success();
        }
    }
}
