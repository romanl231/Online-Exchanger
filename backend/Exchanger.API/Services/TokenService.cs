using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Entities;
using Exchanger.API.Enums.TokenGenerationErrors;
using Exchanger.API.Repositories.IRepositories;
using Exchanger.API.Services.IServices;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Exchanger.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly ISessionTokenRepository _sessionTokenRepository;
        private readonly JWTSettings _jwtSettings;

        public TokenService(ISessionTokenRepository sessionTokenRepository, JWTSettings jwtSettings)
        {
            _sessionTokenRepository = sessionTokenRepository;
            _jwtSettings = jwtSettings;
        }

        public async Task<OnCreationSessionDTO> StartSessionAsync(Guid userId, SessionInfo sessionInfo)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));

            var onCreationDTO = new OnCreationSessionDTO
            {
                AccessToken = GenerateJWT(userId),
                RefreshToken = await GenerateRefreshToken(userId, sessionInfo),
                AccessTokenCookieOptions = GetCookieOptions(TimeSpan
                    .FromMinutes(_jwtSettings.AuthTokenValidityInMinutes)),
                RefreshTokenCookieOptions = GetCookieOptions(TimeSpan
                    .FromDays(_jwtSettings.RefreshTokenValidityInDays)),
            };

            return onCreationDTO;
        }

        public string GenerateJWT(Guid userId)
        {
            var claimsJwt = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claimsJwt,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AuthTokenValidityInMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<string> GenerateRefreshToken(Guid userId, SessionInfo sessionInfo)
        {
            var refreshTokenEntity = new SessionToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenValidityInDays),
                DeviceType = sessionInfo.DeviceType,
                IpAdress = sessionInfo.IpAdress,
                IsRevoked = false,
            };

            if (!await _sessionTokenRepository.AddAsync(refreshTokenEntity))
                throw new InvalidOperationException("An error occured while saving refresh token entity");

            return refreshTokenEntity.Id.ToString();
        }

        public CookieOptions GetCookieOptions(TimeSpan expiresIn)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.Add(expiresIn),
            };

            return cookieOptions;
        }

        public async Task<OnCreationSessionDTO>? RefreshSessionAsync(Guid refreshTokenId, SessionInfo sessionInfo)
        {
            if (refreshTokenId == Guid.Empty)
                throw new ArgumentException("Refresh token ID is empty");

            var session = await _sessionTokenRepository.GetTokenByIdAsync(refreshTokenId);

            if (session == null)
                throw new ArgumentException("Wrong refresh token ID");

            if (session.IsRevoked)
                return null;

            session.IsRevoked = true;
            await _sessionTokenRepository.UpdateTokenAsync(session);

            return await StartSessionAsync(session.UserId, sessionInfo);
        }

        public async Task<bool> RevokeSessionAsync(Guid refreshToken)
        {
            if (refreshToken == Guid.Empty)
                throw new ArgumentException("Wrong refresh token Id");

            var session = await _sessionTokenRepository.GetTokenByIdAsync(refreshToken);

            if (session == null)
                throw new ArgumentException("Session with this ID does not exist");

            if (session.IsRevoked)
                return false;

            session.IsRevoked = true;
            await _sessionTokenRepository.UpdateTokenAsync(session);
            return true;
        }

        public async Task<List<DisplaySessionInfoDTO>> GetSessionsByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Wrong user Id");

            var sessions = await _sessionTokenRepository.GetUnexpiredTokensByUserIdAsync(userId);
            return MapSessionTokensToDTO(sessions);
        }

        public List<DisplaySessionInfoDTO> MapSessionTokensToDTO(List<SessionToken> sessionTokens)
        {
            return sessionTokens.Select(s =>
                new DisplaySessionInfoDTO
                {
                    Id = s.Id,
                    CreatedAt = s.CreatedAt,
                    ExpiresAt = s.ExpiresAt,
                    IpAdress = s.IpAdress,
                    DeviceType = s.DeviceType,
                }).ToList();
        }

        public async Task<TokenResult> GenerateEmailConfirmationTokenAsync(Guid userId)
        {
            var token = GenerateEmailConfirmationJWT(userId);
            return TokenResult.Success(token);
        }

        public string GenerateEmailConfirmationJWT(Guid userId)
        {
            var claimsJwt = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claimsJwt,
                expires: DateTime.UtcNow.AddDays(_jwtSettings.EmailJWTValidityInDays),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<TokenResult> ValidateEmailConfirmationTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(
                token, 
                validationParameters, 
                out SecurityToken validatedToken);

            var userIdClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null)
            {
                return TokenResult.Fail(TokenErrorCode.MissingUserId);
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return TokenResult.Fail(TokenErrorCode.MissingUserId);
            }

            return TokenResult.Success(userId);
        }
    }
}
