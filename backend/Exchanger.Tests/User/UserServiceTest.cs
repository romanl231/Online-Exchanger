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
using Exchanger.API.Services.IServices;
using Microsoft.AspNetCore.Http;
using Exchanger.API.Enums.UploadToCloudErrors;
using CloudinaryDotNet.Actions;

namespace Exchanger.Tests.UserTest
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

            var mockCloudServ = new Mock<ICloudinaryService>();
            var userService = new UserService(mockRepo.Object, mockCloudServ.Object);

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
            var mockCloudServ = new Mock<ICloudinaryService>();
            var userService = new UserService(mockRepo.Object, mockCloudServ.Object);
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
            var mockCloudServ = new Mock<ICloudinaryService>();
            var userService = new UserService(mockRepo.Object, mockCloudServ.Object);

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
            var mockCloudServ = new Mock<ICloudinaryService>();
            var userService = new UserService(mockRepo.Object, mockCloudServ.Object);
            var result = await userService.UpdateUserInfoAsync(updateUserDTO, userId);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldUploadImageToCloud()
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

            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(r => r.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) =>
                {
                    var userToUpdate = users.First(u => u.Id == user.Id);
                    userToUpdate.AvatarUrl = user.AvatarUrl;
                    return true;
                });

            mockUserRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid userId) =>
                {
                    var user = users.FirstOrDefault(u => u.Id == userId);
                    return user;
                });

            var mockCloudServ = new Mock<ICloudinaryService>();

            mockCloudServ.Setup(s => s.UploadImageToCloudAsync(It.IsAny<IFormFile>(), It.IsAny<Guid>()))
                .ReturnsAsync((IFormFile image, Guid userId) =>
                {
                    return CloudResult.Success(GetFakeUploadResult());
                });

            var userService = new UserService(mockUserRepo.Object, mockCloudServ.Object);
            var image = FormFileFactory.CreateFakeFormFile();
            var userId = users.First(u => u.Id != Guid.Empty).Id;
            var result = await userService.UploadAvatarAsync(image, userId);

            Assert.True(result.IsSuccess);
            Assert.NotEqual("MyAvatar", users.FirstOrDefault(u => u.Id == userId).AvatarUrl);
        }

        public static ImageUploadResult GetFakeUploadResult(string publicId = "test_image", string url = "https://res.cloudinary.com/demo/image/upload/v1234567890/test_image.jpg")
        {
            return new ImageUploadResult
            {
                PublicId = publicId,
                Url = new Uri(url),
                SecureUrl = new Uri(url),
                Format = "jpg",
                Width = 800,
                Height = 600,
                Version = "1234567890",
                CreatedAt = DateTime.UtcNow,
                ResourceType = "image",
                Signature = "fake_signature",
                OriginalFilename = "test_image",
                Bytes = 102400,
            };
        }
    }
}
