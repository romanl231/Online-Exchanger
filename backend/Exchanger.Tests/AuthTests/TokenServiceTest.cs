using Exchanger.API.Data;
using Exchanger.API.Entities;
using Microsoft.EntityFrameworkCore;
using Exchanger.API.Services;
using Exchanger.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exchanger.API.Repositories.IRepositories;
using Moq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Exchanger.API.DTOs.AuthDTOs;
using Microsoft.Extensions.Configuration;

namespace Exchanger.Tests.AuthTests
{
    public class TokenServiceTest
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task Should_Start_Session_By_UserId()
        {
            using var context = CreateContext();

            context.Users.AddRange(new List<User>
            {
            new User
            {
                Id = Guid.NewGuid(),
                Email = "examplemail@example.com",
                PasswordHash = "itsMyPasswordHash",
                Name = "MyNameIs",
                Surname = "MyLastname",
                AvatarUrl = "MyAvatar",
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "anotherexampleemail@example.com",
                PasswordHash = "itsMyPasswordHash",
                Name = "MyNameIs",
                Surname = "MyLastname",
                AvatarUrl = "MyAvatar",
            },
            new User {
                Id = Guid.NewGuid(),
                Email = "imtiredofparcingdata@example.com",
                PasswordHash = "itsMyPasswordHash",
                Name = "MyNameIs",
                Surname = "MyLastname",
                AvatarUrl = "MyAvatar",
            }
            });

            await context.SaveChangesAsync();

            var sessionInfo = new SessionInfo
            {
                DeviceType = "Computer",
                IpAdress = "175.174.173.172"
            };

            var mockRepo = new Mock<ISessionTokenRepository>();
            var inMemorySettings = new Dictionary<string, string>
            {
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"},
                {"Jwt:Key", "lJ9IKbvi6gpoCRwW6Eyc7n1EDLyJve9QF4tnO3xGn5c="},
                {"Jwt:TokenValidityInMinutes", "15"},
                {"Jwt:RefreshTokenValidityInDays", "7"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var jwtSettings = new JWTSettings(configuration);

            mockRepo.Setup(repo => repo.AddAsync(It.IsAny<SessionToken>()))
                    .ReturnsAsync(true);

            var tokenService = new TokenService(mockRepo.Object, jwtSettings);

            var existingUserId = context.Users.First().Id;

            var result = await tokenService.StartSessionAsync(existingUserId, sessionInfo);

            var isAccessTokenValid = ValidateJwtToken(
                result.AccessToken,
                jwtSettings.Key,
                jwtSettings.Issuer,
                jwtSettings.Audience);

            Assert.NotNull(result.RefreshToken);
            Assert.NotNull(isAccessTokenValid);
            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<SessionToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_Refresh_JWT_And_Revoke_Session()
        {
            using var context = CreateContext();

            context.Users.AddRange(new List<User>
            {
            new User
            {
                Id = Guid.NewGuid(),
                Email = "examplemail@example.com",
                PasswordHash = "itsMyPasswordHash",
                Name = "MyNameIs",
                Surname = "MyLastname",
                AvatarUrl = "MyAvatar",
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "anotherexampleemail@example.com",
                PasswordHash = "itsMyPasswordHash",
                Name = "MyNameIs",
                Surname = "MyLastname",
                AvatarUrl = "MyAvatar",
            },
            new User {
                Id = Guid.NewGuid(),
                Email = "imtiredofparcingdata@example.com",
                PasswordHash = "itsMyPasswordHash",
                Name = "MyNameIs",
                Surname = "MyLastname",
                AvatarUrl = "MyAvatar",
            }
            });

            await context.SaveChangesAsync();

            context.SessionTokens.AddRange(new List<SessionToken> {
            new SessionToken
            {
                Id = Guid.NewGuid(),
                UserId = context.Users.First().Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                DeviceType = "MyDevice",
                IpAdress = "MyIp",
                IsRevoked = false,
            },
            new SessionToken
            {
                Id = Guid.NewGuid(),
                UserId = context.Users.FirstOrDefault(u => u.Email == "anotherexampleemail@example.com").Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                DeviceType = "MyDevice",
                IpAdress = "MyIp",
                IsRevoked = true,
            },
            });

            await context.SaveChangesAsync();

            var sessionInfo = new SessionInfo
            {
                DeviceType = "Computer",
                IpAdress = "175.174.173.172"
            };

            var sessionTokens = context.SessionTokens.ToList();

            var mockRepo = new Mock<ISessionTokenRepository>();

            mockRepo.Setup(r => r.GetTokenByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => sessionTokens.FirstOrDefault(t => t.Id == id));

            mockRepo.Setup(r => r.AddAsync(It.IsAny<SessionToken>()))
                .ReturnsAsync((SessionToken token) =>
                {
                    sessionTokens.Add(token);
                    return true;
                });

            mockRepo.Setup(r => r.UpdateTokenAsync(It.IsAny<SessionToken>()))
                .Returns((SessionToken token) =>
                {
                    var index = sessionTokens.FindIndex(t => t.Id == token.Id);
                    if (index != -1)
                    {
                        sessionTokens[index] = token;
                    }
                    return Task.CompletedTask;
                });

            var inMemorySettings = new Dictionary<string, string>
            {
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"},
                {"Jwt:Key", "lJ9IKbvi6gpoCRwW6Eyc7n1EDLyJve9QF4tnO3xGn5c="},
                {"Jwt:TokenValidityInMinutes", "15"},
                {"Jwt:RefreshTokenValidityInDays", "7"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var jwtSettings = new JWTSettings(configuration);


            var tokenService = new TokenService(mockRepo.Object, jwtSettings);

            var unRevokedSessionId = context.SessionTokens
                .Where(s => s.IsRevoked == false)
                .Select(s => s.Id)
                .FirstOrDefault();

            var resultUnRevoked = await tokenService.RefreshSessionAsync(unRevokedSessionId, sessionInfo);

            var isAccessTokenValid = ValidateJwtToken(
                resultUnRevoked.AccessToken,
                jwtSettings.Key,
                jwtSettings.Issuer,
                jwtSettings.Audience);

            Assert.NotNull( isAccessTokenValid );
            Assert.NotNull( resultUnRevoked.RefreshToken );

            var revokedSessionId = context.SessionTokens
                .Where(s => s.IsRevoked == true)
                .Select(s => s.Id)
                .FirstOrDefault();
            var resultRevoked = await tokenService.RefreshSessionAsync(revokedSessionId, sessionInfo);
            Assert.Null(resultRevoked);
        }

        public ClaimsPrincipal? ValidateJwtToken(string token, string secret, string issuer, string audience)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secret);

            try
            {
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = true,
                    ValidIssuer = issuer,

                    ValidateAudience = true,
                    ValidAudience = audience,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Token validation failed: " + ex.Message);
                return null;
            }
        }
    }
}
