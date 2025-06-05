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
using Microsoft.Extensions.Logging;
using Moq;

namespace Exchanger.Tests.ListingTests
{
    public class ListingServiceTest
    {
        private AppDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // унікальне ім'я щоб уникнути конфліктів
                .EnableSensitiveDataLogging()
                .Options;

            var ctx = new AppDbContext(options);

            ctx.Database.EnsureDeleted(); // очищення
            ctx.Database.EnsureCreated(); // створення

            return ctx;
        }

        private async Task SeedDbAsync(AppDbContext context)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "testuser@example.com"
            };

            var categories = new List<Category>
            {
                new Category { Name = "Category 1" },
                new Category { Name = "Category 2" }
            };

            await context.Users.AddAsync(user);
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task ShouldCreateNewListing()
        {
            using var context = CreateInMemoryContext();

            try
            {
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

                var category = context.Categories.FirstOrDefault();

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
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
