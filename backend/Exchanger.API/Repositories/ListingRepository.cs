using Exchanger.API.Data;
using Exchanger.API.DTOs.ListingDTOs;
using Exchanger.API.Entities;
using Exchanger.API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Exchanger.API.DTOs.AuthDTOs;
using static System.Net.Mime.MediaTypeNames;

namespace Exchanger.API.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private readonly AppDbContext _context;

        public ListingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(
            Listing listing,
            List<ListingImages> images,
            List<ListingCategory> categories)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Listing.Add(listing);
                _context.ListingImages.AddRange(images);
                _context.ListingCategories.AddRange(categories);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        public async Task<List<DisplayListingDTO>> GetListingInfoByUserIdAsync(
            Guid userId,
            Guid? lastListingId,
            int limit)
        { 
            var query = _context.Listing
                .Where(l => l.UserId == userId);

            if (lastListingId.HasValue)
            {
                var lastCreatedAt = _context.Listing
                    .Where(l => l.Id == lastListingId.Value)
                    .Select(l => l.Created)
                    .FirstOrDefault();
                query = query.Where(l => l.Created < lastCreatedAt);
            }

            var page = await query
                .OrderByDescending(l => l.Created)
                .Include(l => l.User)
                .Include(l => l.Categories).ThenInclude(lc => lc.Category)
                .Include(l => l.Images)
                .Select(l => new DisplayListingDTO
                {
                    ListingId       = l.Id,
                    Title           = l.Title,
                    Description     = l.Description,
                    Price           = l.Price,
                    CreatedAt       = l.Created,
                    UpdatedAt       = l.Updated,
                    PublisherInfo   = new DisplayUserInfoDTO
                    {
                        Email       = l.User.Email,
                        FirstName   = l.User.Name,
                        Surname     = l.User.Surname,
                        AvatarUrl   = l.User.AvatarUrl,
                    },
                    Categories = l.Categories
                        .Select(lc => lc.Category)
                        .ToList(),
                    ImageUrls = l.Images
                        .Select(l => l.ImageUrl)
                        .ToList(),
                })
                .Take(limit)
                .ToListAsync();

            return page
                .OrderBy(dto => dto.CreatedAt)
                .ToList();
        }

        public async Task<List<DisplayListingDTO>> GetListingByParamsAsync(ListingParams listingParams)
        {
            return new List<DisplayListingDTO>();
        }

        public async Task<List<DisplayListingDTO>> SearchByTitleAsync(string title)
        {
            return new List<DisplayListingDTO>();
        }

        public async Task<Listing?> GetListingByIdAsync(Guid listingId)
        {
            var listing = await _context.Listing.FirstOrDefaultAsync(l => l.Id == listingId);
            return listing;
        }

        public async Task<DisplayListingDTO?> GetListingInfoByIdAsync(Guid listingId)
        {
            var listing = await _context.Listing
                .Where(l => l.Id == listingId)
                .Include(l => l.User)
                .Include(l => l.Categories).ThenInclude(lc => lc.Category)
                .Include(l => l.Images)
                .Select(l => new DisplayListingDTO
                {
                    ListingId = l.Id,
                    Title = l.Title,
                    Description = l.Description,
                    Price = l.Price,
                    CreatedAt = l.Created,
                    UpdatedAt = l.Updated,
                    PublisherInfo = new DisplayUserInfoDTO
                    {
                        Email = l.User.Email,
                        FirstName = l.User.Name,
                        Surname = l.User.Surname,
                        AvatarUrl = l.User.AvatarUrl,
                    },
                    Categories = l.Categories
                        .Select(lc => lc.Category)
                        .ToList(),
                    ImageUrls = l.Images
                        .Select(l => l.ImageUrl)
                        .ToList(),
                })
                .FirstOrDefaultAsync();

            return listing;
        }

        public async Task<bool> AddImageAsync(List<ListingImages> images)
        {
            _context.ListingImages.AddRange(images);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteImageAsync(Guid listingId, string imageUrl)
        {
            var image = await _context.ListingImages
                .FirstOrDefaultAsync(lc =>
                lc.ListingId == listingId &&
                lc.ImageUrl == imageUrl);
            _context.ListingImages.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddCategoryAsync(List<ListingCategory> categories)
        {
            _context.ListingCategories.AddRange(categories);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(Guid listingId, int categoryId)
        {
            var listingCategory = await _context.ListingCategories
                .FirstOrDefaultAsync(lc => 
                lc.ListingId == listingId && 
                lc.CategoryId == categoryId);
            if (listingCategory == null)
            {
                return false;
            }
            _context.ListingCategories.Remove(listingCategory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateListingAsync(Listing listing)
        {
            _context.Update(listing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteListingAsync(Listing listing)
        {
            _context.Listing.Remove(listing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return 
                await _context.
                Categories.
                ToListAsync();
        }
    }
}
