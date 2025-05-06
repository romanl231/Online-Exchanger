using Exchanger.API.Data;
using Exchanger.API.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Exchanger.API.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exchanger.API.Services;
using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Enums.AuthErrors;
using Azure;

namespace Exchanger.Tests.AuthTests
{
    public class UserServiceTest
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task Should_Register_User_To_DB()
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
            var users = context.Users.ToList();
            var mockRepo = new Mock<IUserRepository>();

            mockRepo.Setup(r => r.AddAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) =>
                {
                    users.Add(user);
                    return true;
                });

            var userService = new UserService(mockRepo.Object);

            var authDto = new AuthDTO
            {
                Email = "pososachka@example.com",
                Password = "my#PassWord1",
            };

            var response1 = await userService.RegisterUserAsync(authDto);

            var userId = users.First().Id;
            var expectedResponse = new User
            {
                Id = userId,
                Email = "pososachka@example.com",
                PasswordHash = "somehash",
                Name = "None",
                Surname = "None",
                AvatarUrl = "None",
            };

            Assert.Equal(AuthResult.Success(expectedResponse), response1);

            var response2 = await userService.RegisterUserAsync(authDto);
            Assert.Equal(AuthResult.Fail(AuthErrorCode.EmailUsed), response2);
        }

        [Fact]
        public async Task Validate_Password_Test()
        {
            using var context = CreateContext();
            var users = context.Users.ToList();

            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.AddAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) =>
                {
                    users.Add(user);
                    return true;
                });

            var userService = new UserService(mockRepo.Object);
            var authDto = new AuthDTO
            {
                Email = "pososachka@example.com",
                Password = "mypass",
            };
            var result1 = await userService.RegisterUserAsync(authDto);
            Assert.Equal(AuthResult.Fail(AuthErrorCode.ShortPassword), result1);

            authDto.Password = "qwertyui";
            var result2 = await userService.RegisterUserAsync(authDto);
            Assert.Equal(AuthResult.Fail(AuthErrorCode.PasswordMustContainNum), result2);

            authDto.Password = "qwertyui1";
            var result3 = await userService.RegisterUserAsync(authDto);
            Assert.Equal(AuthResult.Fail(AuthErrorCode.PasswordMustContainSpecialChar), result3);

            authDto.Password = "qwertyui1#";
            var result4 = await userService.RegisterUserAsync(authDto);
            Assert.Equal(AuthResult.Fail(AuthErrorCode.PasswordMustContainUppercase), result4);

            authDto.Password = "12345678";
            var result5 = await userService.RegisterUserAsync(authDto);
            Assert.Equal(AuthResult.Fail(AuthErrorCode.PasswordMustContainChar), result5);

            authDto.Password = "qwertyui1#Q";
            var result6 = await userService.RegisterUserAsync(authDto);
            Assert.NotNull(result6);
        }
    }
}
