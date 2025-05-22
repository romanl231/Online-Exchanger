using CloudinaryDotNet.Actions;
using Exchanger.API.Data;
using Exchanger.API.DTOs.ListingDTOs;
using Exchanger.API.Entities;
using Exchanger.API.Enums.UploadToCloudErrors;
using Exchanger.API.Repositories.IRepositories;
using Exchanger.API.Services;
using Exchanger.API.Services.IServices;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchanger.Tests.ListingTests
{
    public class ListingServiceTest
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private AppDbContext CreateSqliteContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            var ctx = new AppDbContext(options);
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();
            return ctx;
        }

        private async Task<(Guid userId, List<Listing> listings)> SeedDbAsync(AppDbContext context)
        {
            var users = new List<User>
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
        };

            var categories = new List<Category>
            {
                new Category { Name = "Name1" },
                new Category { Name = "Name2" },
                new Category { Name = "Name3" }
            };

            await context.Users.AddRangeAsync(users);
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            var userId = users.First().Id;
            var listings = new List<Listing>();

            for (int i = 0; i < 30; i++)
            {
                var listingId = Guid.NewGuid();
                var listing = new Listing
                {
                    Id = listingId,
                    Title = $"Title {i}",
                    Description = $"Description {i}",
                    UserId = userId,
                    IsActive = true,
                    Price = 10 + i,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                };

                var images = new List<ListingImages>
                {
                new ListingImages { Id = Guid.NewGuid(), ImageUrl = $"Url_{i}_1", ListingId = listingId },
                new ListingImages { Id = Guid.NewGuid(), ImageUrl = $"Url_{i}_2", ListingId = listingId }
                };

                var listingCategories = new List<ListingCategory>
                {
                new ListingCategory { CategoryId = 1, ListingId = listingId },
                new ListingCategory { CategoryId = 2, ListingId = listingId },
                };

                await context.ListingImages.AddRangeAsync(images);
                await context.ListingCategories.AddRangeAsync(listingCategories);

                listings.Add(listing);
            }

            await context.Listing.AddRangeAsync(listings);
            await context.SaveChangesAsync();

            return (userId, listings);
        }

        [Fact]
        public async Task ShouldCreateNewListing()
        {
            using var context = CreateSqliteContext();
            await SeedDbAsync(context);

            var mockRepository = new Mock<IListingRepository>();
            mockRepository.Setup(r => r.AddAsync(It.IsAny<Listing>(), It.IsAny<List<ListingImages>>(), It.IsAny<List<ListingCategory>>()))
                .ReturnsAsync((Listing listing, List<ListingImages> images, List<ListingCategory> categories) =>
                {
                    context.Listing.Add(listing);
                    context.ListingImages.AddRange(images);
                    context.ListingCategories.AddRange(categories);
                    context.SaveChanges();
                    return true;
                });
            var mockCloudinaryService = new Mock<ICloudinaryService>();

            
            var fakeUploadResult = new ImageUploadResult
            {
                SecureUrl = new Uri("http://res.cloudinary.com/fake/image.jpg"),
                PublicId = "some_public_id"
            };

            mockCloudinaryService.Setup(s => s.UploadListingImagesToCloudAsync(
                It.IsAny<List<IFormFile>>(), 
                It.IsAny<Guid>(), 
                It.IsAny<Guid>()))
                .ReturnsAsync((List<IFormFile> images, Guid userId, Guid listingId) =>
                {
                    var cloudinaryResults = images.Select(_ => CloudResult.Success(fakeUploadResult)).ToArray();
                    return cloudinaryResults;
                });
            
            var service = new ListingService(mockRepository.Object, mockCloudinaryService.Object);
            var images = new List<IFormFile>();

            for (int i = 0; i < 5; i++)
            {
                images.Add(FormFileFactory.CreateFakeFormFile());
            }

            var listingCreationDto = new ListingCreationDTO
            {
                Title = "TitleSpecial",
                Description = "Description",
                CategoryIds = new List<int> { 1, 2 },
                Price = 10m,
                Images = images,
            };

            var userId = context.Users.First().Id;
            var result = await service.CreateNewListingAsync(listingCreationDto, userId);

            result.IsSuccess.Should().BeTrue();
            var tryFind = context.Listing.Where(l => l.Title == "TitleSpecial")
                .Include(l => l.Images)
                .Include(l => l.Categories)
                .ThenInclude(lc => lc.Category)
                .FirstOrDefault();

            tryFind.Should().NotBeNull();
            tryFind.Images.Should().NotBeEmpty();
            tryFind.Categories.Should().NotBeEmpty();
        }
    }
}
