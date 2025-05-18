using Exchanger.API.Data;
using Exchanger.API.DTOs.ListingDTOs;
using Exchanger.API.Entities;
using Exchanger.API.Repositories.IRepositories;

namespace Exchanger.API.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private readonly AppDbContext _appDbContext;

        public ListingRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AddAsync(
            Listing listing,
            List<ListingImages> images,
            List<ListingCategory> categories)
        {
            return false;
        }

        public async Task<List<DisplayListingDTO>> GetListingInfoByUserIdAsync(
            Guid userId,
            Guid? lastListingId,
            int limit)
        {
            return new List<DisplayListingDTO>();
        }

        public async Task<List<DisplayListingDTO>> GetListingByParamsAsync(ListingParams listingParams)
        {
            return new List<DisplayListingDTO>();
        }

        public async Task<List<DisplayListingDTO>> SearchByTitleAsync(string title)
        {
            return new List<DisplayListingDTO>();
        }

        public async Task<Listing> GetListingByIdAsync(Guid listingId)
        {
            return new Listing();
        }

        public async Task<DisplayListingDTO> GetListingInfoByIdAsync(Guid listingId)
        {
            return new DisplayListingDTO();
        }

        public async Task<bool> AddImageAsync(List<ListingImages> images)
        {
            return false;
        }

        public async Task<bool> DeleteImageAsync(Guid listingId, string imageUrl)
        {
            return false;
        }

        public async Task<bool> AddCategoryAsync(List<Category> categories)
        {
            return false;
        }

        public async Task<bool> DeleteCategoryAsync(Guid listingId, int categoryId)
        {
            return false;
        }

        public async Task<bool> UpdateListingAsync(Listing listing)
        {
            return false;
        }

        public async Task<bool> DeleteListingAsync(Listing listing)
        {
            return false;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return new List<Category>();
        }
    }
}
