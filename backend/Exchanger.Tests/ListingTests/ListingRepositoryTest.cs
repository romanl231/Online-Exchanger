using Exchanger.API.Data;
using Exchanger.API.Entities;
using Exchanger.API.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchanger.Tests.ListingTests
{
    public class ListingRepositoryTest
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task Should_Create_Listing()
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

            context.Categories.AddRange(new List<Category> {
                new Category
                {
                    Id = 1,
                    Name = "Name",
                },
                new Category {
                    Id = 2,
                    Name = "Name",
                },
                new Category {
                    Id = 3,
                    Name = "Name",
                }
            });

            await context.SaveChangesAsync();

            var listingEntity = new Listing
            {
                Id = Guid.NewGuid(),
                Description = "Description",
                Title = "Title",
                Price = 14.88m,
                UserId = context.Users.First().Id,
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
                },
                new ListingImages
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = "NewUrl",
                    ListingId = listingEntityId,
                },
                new ListingImages
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = "AnotherUrl",
                    ListingId = listingEntityId,
                }
            };

            var listingCategories = new List<ListingCategory> {
                new ListingCategory
                {
                    CategoryId = 1,
                    ListingId = listingEntityId,
                },
                new ListingCategory
                {
                    CategoryId = 2,
                    ListingId = listingEntityId,
                },
                new ListingCategory
                {
                    CategoryId = 3,
                    ListingId = listingEntityId,
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
            Assert.NotNull( listing);
            var listingImagesCheck = context.ListingImages.First();
            Assert.NotNull( listingImagesCheck);
            var listingImageCategory = context.ListingCategories.First();
            Assert.NotNull( listingImageCategory);
        }

        [Fact]
        public async Task Should_GetPaginatedListing()
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

            context.Categories.AddRange(new List<Category> {
                new Category
                {
                    Id = 1,
                    Name = "Name",
                },
                new Category {
                    Id = 2,
                    Name = "Name",
                },
                new Category {
                    Id = 3,
                    Name = "Name",
                }
            });

            await context.SaveChangesAsync();

            var userId = context.Users.First().Id;
            var listings = new List<Listing>();

            for (int i = 0; i < 30; i++)
            {
                listings.Add(new Listing
                {
                    Id = Guid.NewGuid(),
                    Title = i.ToString(),
                    Description = i.ToString(),
                    UserId = userId,
                    IsActive = true,
                    Price = 10m,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                });
            }

            context.AddRange(listings);
            context.SaveChanges();

            var listingRepository = new ListingRepository(context);
            var result = await listingRepository.GetListingInfoByUserIdAsync(userId, null, 15);
            var lastGuid = result.Last().ListingId;
            result.Should().NotBeEmpty();
            result.Should().HaveCount(15);
            var result2 = await listingRepository.GetListingInfoByUserIdAsync(userId, lastGuid, 15);
            result2.Should().NotBeEmpty();
            result2.Should().HaveCount(15);
        }
    }
}
