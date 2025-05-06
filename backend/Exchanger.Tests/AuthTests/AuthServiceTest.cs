using Exchanger.API.Data;
using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Entities;
using Exchanger.API.Enums.AuthErrors;
using Exchanger.API.Repositories.IRepositories;
using Exchanger.API.Services;
using Exchanger.API.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchanger.Tests.AuthTests
{
    public class AuthServiceTest
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task Should_Return_Tokens_If_Success_Or_AuthFail_If_Smth_wrong()
        {
            using var context = CreateContext();

            context.Users.AddRange(new List<User>
            {
            new User
            {
                Id = Guid.NewGuid(),
                Email = "examplemail@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("itsMyPasswordHash"),
                Name = "MyNameIs",
                Surname = "MyLastname",
                AvatarUrl = "MyAvatar",
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "anotherexampleemail@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("itsMyPasswordHash"),
                Name = "MyNameIs",
                Surname = "MyLastname",
                AvatarUrl = "MyAvatar",
            },
            new User {
                Id = Guid.NewGuid(),
                Email = "imtiredofparcingdata@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("itsMyPasswordHash"),
                Name = "MyNameIs",
                Surname = "MyLastname",
                AvatarUrl = "MyAvatar",
            }
            });



            await context.SaveChangesAsync();
            var users = context.Users.ToList();
            var tokens = context.SessionTokens.ToList();

            var mockUserServ = new Mock<IUserService>();
            var mockTokenServ = new Mock<ITokenService>();

            mockUserServ.Setup(s => s.CheckDoesPasswordsMatches(It.IsAny<AuthDTO>()))
                .ReturnsAsync((AuthDTO authDTO) =>
                {
                    var user = users.FirstOrDefault(u => u.Email == authDTO.Email);
                    if(user == null)
                        return AuthResult.Fail(AuthErrorCode.UserNotFound);

                    if(!BCrypt.Net.BCrypt.Verify(authDTO.Password, user.PasswordHash))
                        return AuthResult.Fail(AuthErrorCode.InvalidCredentials);

                    return AuthResult.Success(user);
                });

            var cookieOpt = new CookieOptions(); 

            mockTokenServ.Setup(s => s.StartSessionAsync(It.IsAny<Guid>(), It.IsAny<SessionInfo>()))
                .ReturnsAsync((Guid userId, SessionInfo sessionInfo) =>
                {
                    return new OnCreationSessionDTO
                    {
                        AccessToken = "someaccesstoken",
                        RefreshToken = "somerefreshtoken",
                        AccessTokenCookieOptions = cookieOpt,
                        RefreshTokenCookieOptions = cookieOpt,
                    };
                });
                    
            var authService = new AuthService(mockUserServ.Object, mockTokenServ.Object);

            var authDto = new AuthDTO
            {
                Email = "anotherexampleemail@example.com",
                Password = "adwadasdads"
            };

            var sessionInfo = new SessionInfo
            {
                DeviceType = "Compuder",
                IpAdress = "Compuder aderese",
            };

            var result1 = await authService.LoginAsync(authDto, sessionInfo);
            Assert.Equal(AuthErrorCode.InvalidCredentials, result1.ErrorCode);

            authDto.Password = "itsMyPasswordHash";
            var result2 = await authService.LoginAsync(authDto, sessionInfo);
            Assert.Equal("someaccesstoken", result2.OnCreationSession.AccessToken);
            Assert.Equal("somerefreshtoken", result2.OnCreationSession.RefreshToken);

            authDto.Email = "someshit@email.example";
            var result3 = await authService.LoginAsync(authDto,sessionInfo);
            Assert.Equal(AuthErrorCode.UserNotFound, result3.ErrorCode);
        }
    }
}
