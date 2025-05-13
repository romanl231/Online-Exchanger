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

            mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) =>
                {
                    var user = users.FirstOrDefault(u => u.Email == email);
                    return user;
                });

            var userService = new UserService(mockRepo.Object);

            var authDto = new AuthDTO
            {
                Email = "pososachka@example.com",
                Password = "my#PassWord1",
            };

            var response1 = await userService.RegisterUserAsync(authDto);

            var user = users.FirstOrDefault(u => u.Email == "pososachka@example.com");

            Assert.NotNull(user);
            Assert.NotEqual(Guid.Empty, user.Id);

            var response2 = await userService.RegisterUserAsync(authDto);
            Assert.Equal(AuthErrorCode.EmailUsed, response2.ErrorCode);
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
            Assert.Equal(AuthErrorCode.ShortPassword, result1.ErrorCode);

            authDto.Password = "qwertyui";
            var result2 = await userService.RegisterUserAsync(authDto);
            Assert.Equal(AuthErrorCode.PasswordMustContainNum, result2.ErrorCode);

            authDto.Password = "qwertyui1";
            var result3 = await userService.RegisterUserAsync(authDto);
            Assert.Equal(AuthErrorCode.PasswordMustContainSpecialChar, result3.ErrorCode);

            authDto.Password = "qwertyui1#";
            var result4 = await userService.RegisterUserAsync(authDto);
            Assert.Equal(AuthErrorCode.PasswordMustContainUppercase, result4.ErrorCode);

            authDto.Password = "12345678";
            var result5 = await userService.RegisterUserAsync(authDto);
            Assert.Equal(AuthErrorCode.PasswordMustContainChar, result5.ErrorCode);

            authDto.Password = "qwertyui1#Q";
            var result6 = await userService.RegisterUserAsync(authDto);
            Assert.NotNull(result6);
        }

        [Fact]
        public async Task Get_User_Info()
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
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid userId) =>
                {
                    var user = users.FirstOrDefault(u => u.Id == userId);
                    return user;
                });

            var user = users.First(u => u.Id != Guid.Empty);
            var userService = new UserService(mockRepo.Object);
            var userInfo = await userService.GetUserInfoAsync(user.Id);

            Assert.NotNull(userInfo);
            Assert.NotNull(userInfo.FirstName); 
            Assert.NotNull(userInfo.Surname);
            Assert.NotNull(userInfo.Email);
            Assert.NotNull(userInfo.AvatarUrl);
        }

        [Fact]
        public async Task Should_Update_User_Entity()
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
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) =>
                {
                    var userToUpdate = users.First(u => u.Id == user.Id);
                    userToUpdate.Name = user.Name;
                    userToUpdate.Surname = user.Surname;
                    return true;
                });

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid userId) =>
                {
                    var user = users.FirstOrDefault(u => u.Id == userId);
                    return user;
                });

            var userId = users.First(u => u.Id != Guid.Empty).Id;
            var updateUserDTO = new UpdateProfileDTO
            {
                Name = "Eminem",
                Surname = "MarshallMathew"
            };

            var userService = new UserService(mockRepo.Object);
            var result = await userService.UpdateUserInfoAsync(updateUserDTO, userId);

            Assert.True(result.IsSuccess);
        }
    }
}
