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
            }, 
            new User
            {
                Id = Guid.NewGuid(),
                Email = "anotherexampleemail@example.com",
                PasswordHash = "itsMyPasswordHash",
            },
            new User {
                Id = Guid.NewGuid(),
                Email = "imtiredofparcingdata@example.com",
                PasswordHash = "itsMyPasswordHash",
            }
            });

            await context.SaveChangesAsync();

            var mockRepo = new Mock<ISessionTokenRepository>();

            mockRepo.Setup(repo => repo.AddAsync(It.IsAny<SessionToken>()))
                    .ReturnsAsync(true);

            var tokenService = new TokenService(mockRepo.Object);

            var result = await tokenService.StartSessionAsync(Guid.NewGuid());

            var isAccessTokenValid = ValidateJwtToken(result.AccessToken, "key");
            var isRefreshTokenValid = ValidateJwtToken(result.RefreshToken, "key");
            
            Assert.NotNull(isAccessTokenValid);
            Assert.NotNull(isRefreshTokenValid);
            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<SessionToken>()), Times.Once);
        }

        public ClaimsPrincipal? ValidateJwtToken(string token, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(secret);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,

                    ValidateLifetime = true 
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
