﻿using Exchanger.API.Data;
using Exchanger.API.DTOs.ListingDTOs;
using Exchanger.API.Entities;
using Exchanger.API.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace Exchanger.Tests.ListingTests
{
    public class ListingRepositoryTest
    {
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

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            var user = users.First();
            var listings = new List<Listing>();

            for (int i = 0; i < 30; i++)
            {
                var listingId = Guid.NewGuid();
                var listing = new Listing
                {
                    Id = listingId,
                    Title = $"Title {i}",
                    Description = $"Description {i}",
                    UserId = user.Id,
                    User = user,
                    IsActive = true,
                    Price = 10 + i,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                };

                var images = new List<ListingImages>
                {
                    new ListingImages { Id = Guid.NewGuid(), ImageUrl = $"Url_{i}_1", ListingId = listingId, Listing = listing },
                    new ListingImages { Id = Guid.NewGuid(), ImageUrl = $"Url_{i}_2", ListingId = listingId, Listing = listing }
                };

                var listingCategories = new List<ListingCategory>
                {
                    new ListingCategory { CategoryId = 1, ListingId = listingId, Listing = listing },
                    new ListingCategory { CategoryId = 2, ListingId = listingId, Listing = listing },
                };

                await context.ListingImages.AddRangeAsync(images);
                await context.ListingCategories.AddRangeAsync(listingCategories);

                listings.Add(listing);
            }

            await context.Listing.AddRangeAsync(listings);
            await context.SaveChangesAsync();

            return (user.Id, listings);
        }

        private AppDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            var ctx = new AppDbContext(options);

            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();

            return ctx;
        }

        public async Task SeedCategoriesIfNotExist(AppDbContext context)
        {
            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Name1" },
                    new Category { Name = "Name2" },
                    new Category { Name = "Name3" }
                };
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
        }


        [Fact]
        public async Task Should_Create_Listing()
        {
            using var context = await CreateSqliteContext();

            var user = new User { Id = Guid.NewGuid(), Email = "a@b.c", PasswordHash = "x", Name = "N", Surname = "S", AvatarUrl = "" };
            context.Users.Add(user);

            var listings = new[]
            {
                    new Listing {
                        Id=Guid.NewGuid(),
                        Title="Title 1",
                        Description="",
                        UserId=user.Id,
                        IsActive=true,
                        Price=1,
                        Created=DateTime.UtcNow,
                        Updated=DateTime.UtcNow },
                    new Listing {
                        Id=Guid.NewGuid(),
                        Title="Title 2", Description="",
                        UserId=user.Id,
                        IsActive=true,
                        Price=2,
                        Created=DateTime.UtcNow,
                        Updated=DateTime.UtcNow
                    },
                    new Listing {
                        Id=Guid.NewGuid(),
                        Title="Best Title Ever",
                        Description="",
                        UserId=user.Id,
                        IsActive=true,
                        Price=3,
                        Created=DateTime.UtcNow,
                        Updated=DateTime.UtcNow
                    },
                    new Listing {
                        Id=Guid.NewGuid(),
                        Title="Nothing to do with query",
                        Description="",
                        UserId=user.Id,
                        IsActive=true,
                        Price=4,
                        Created=DateTime.UtcNow,
                        Updated=DateTime.UtcNow
                    }
                };
            context.Listing.AddRange(listings);
            await context.SaveChangesAsync();

            var listingEntity = new Listing
            {
                Id = Guid.NewGuid(),
                Description = "Description",
                Title = "Title",
                Price = 14,
                UserId = user.Id,
                User = user,
                IsActive = true,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
            };

            var listingEntityId = listingEntity.Id;

            var listingImages = new List<ListingImages>
            {
                new ListingImages {
                    Id = Guid.NewGuid(),
                    ImageUrl = "Url",
                    ListingId = listingEntityId,
                    Listing = listingEntity,
                },
                new ListingImages
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = "NewUrl",
                    ListingId = listingEntityId,
                    Listing = listingEntity,
                },
                new ListingImages
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = "AnotherUrl",
                    ListingId = listingEntityId,
                    Listing = listingEntity,
                }
            };

            var categories = context.Categories.ToList();
            var listingCategories = new List<ListingCategory> {
                new ListingCategory
                {
                    CategoryId = 1,
                    Category = context.Categories.FirstOrDefault(c => c.Id == 1),
                    ListingId = listingEntityId,
                    Listing = listingEntity,
                },
                new ListingCategory
                {
                    CategoryId = 2,
                    Category = context.Categories.FirstOrDefault(c => c.Id == 2),
                    ListingId = listingEntityId,
                    Listing = listingEntity,
                },
                new ListingCategory
                {
                    CategoryId = 3,
                    Category = context.Categories.FirstOrDefault(c => c.Id == 3),
                    ListingId = listingEntityId,
                    Listing = listingEntity,
                }
            };

            var listingRepository = new ListingRepository(context);
            var result = await listingRepository.AddAsync(
                listingEntity,
                listingImages,
                listingCategories
                );

            Assert.True(result);
            var listing = context.Listing.First();
            Assert.NotNull(listing);
            var listingImagesCheck = context.ListingImages.First();
            Assert.NotNull(listingImagesCheck);
            var listingImageCategory = context.ListingCategories.First();
            Assert.NotNull(listingImageCategory);
        }

        [Fact]
        public async Task Should_GetPaginatedListing()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var (userId, seededListings) = await SeedDbAsync(context);
            // SeedDbAsync already saves users, categories, and initial listings

            // Add additional listings
            var newListings = new List<Listing>();
            for (int i = 0; i < 30; i++)
            {
                newListings.Add(new Listing
                {
                    Id = Guid.NewGuid(),
                    Title = i.ToString(),
                    Description = i.ToString(),
                    UserId = userId,
                    IsActive = true,
                    Price = 10m,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                });
            }

            await context.Listing.AddRangeAsync(newListings);
            await context.SaveChangesAsync();

            var listingRepository = new ListingRepository(context);

            // Act & Assert - first page
            var firstPage = await listingRepository.GetListingInfoByUserIdAsync(userId, null, 15);
            firstPage.Should().NotBeEmpty();
            firstPage.Should().HaveCount(15);

            // Act & Assert - second page
            var lastGuidOfFirstPage = firstPage.Last().ListingId;
            var secondPage = await listingRepository.GetListingInfoByUserIdAsync(userId, lastGuidOfFirstPage, 15);
            secondPage.Should().NotBeEmpty();
            secondPage.Should().HaveCount(15);
        }

        [Fact]
        public async Task Should_Return_By_Params()
        {
            using var context = CreateInMemoryContext();
            var (userId, listings) = await SeedDbAsync(context);
            var listingParams = new ListingParams
            {
                MinValue = 13m,
                MaxValue = 18m,
                Pagination = new PaginationDTO
                {
                    LastId = null,
                    Limit = 15
                }
            };

            var category = context.Categories.First();

            var listingRepository = new ListingRepository(context);
            var result = await listingRepository.GetListingByParamsAsync(listingParams);
            result.Should().NotBeEmpty();
            result[1].Price.Should().BeInRange(13m, 18m);
            result.Should().OnlyContain(l => l.Price >= 13m && l.Price <= 18m);
        }

        private async Task<AppDbContext> CreateSqliteContext()
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

        [Fact]
        public async Task SearchByTitleAsync_ShouldReturn_ExactAndFuzzyMatches()
        {
            // Arrange
            await using var context = await CreateSqliteContext();

            var user = new User { Id = Guid.NewGuid(), Email = "a@b.c", PasswordHash = "x", Name = "N", Surname = "S", AvatarUrl = "" };
            context.Users.Add(user);

            var listings = new[]
            {
                    new Listing { 
                        Id=Guid.NewGuid(), 
                        Title="Title 1", 
                        Description="", 
                        UserId=user.Id, 
                        IsActive=true, 
                        Price=1, 
                        Created=DateTime.UtcNow, 
                        Updated=DateTime.UtcNow },
                    new Listing { 
                        Id=Guid.NewGuid(), 
                        Title="Title 2", Description="", 
                        UserId=user.Id, 
                        IsActive=true, 
                        Price=2, 
                        Created=DateTime.UtcNow, 
                        Updated=DateTime.UtcNow 
                    },
                    new Listing { 
                        Id=Guid.NewGuid(), 
                        Title="Best Title Ever", 
                        Description="", 
                        UserId=user.Id, 
                        IsActive=true, 
                        Price=3, 
                        Created=DateTime.UtcNow, 
                        Updated=DateTime.UtcNow 
                    },
                    new Listing { 
                        Id=Guid.NewGuid(), 
                        Title="Nothing to do with query", 
                        Description="", 
                        UserId=user.Id, 
                        IsActive=true, 
                        Price=4, 
                        Created=DateTime.UtcNow, 
                        Updated=DateTime.UtcNow 
                    }
                };
            context.Listing.AddRange(listings);
            await context.SaveChangesAsync();

            var repo = new ListingRepository(context);

            // Act: запит з помилкою (ми пропустили одну літеру “i” — “Ttle 1”)
            var result = await repo.SearchByTitleAsync("Ttle 1", null, 15);

            // Assert
            // має знайти “Title 1” завдяки EF.Functions.Like
            result.Should().Contain(dto => dto.Title == "Title 1");

            // має знайти “Best Title Ever” завдяки нечіткій відповідності (FuzzySharp)
            result.Should().Contain(dto => dto.Title == "Best Title Ever");

            // не має містити запис із зовсім нерелевантним заголовком
            result.Should().NotContain(dto => dto.Title == "Nothing to do with query");

            // перевіримо, що кількість результатів не надто велика (наприклад, не більше трьох)
            result.Count.Should().BeLessThanOrEqualTo(3);
        }
    }
}
