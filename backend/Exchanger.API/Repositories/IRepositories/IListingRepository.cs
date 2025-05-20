using Exchanger.API.DTOs.ListingDTOs;
using Exchanger.API.Entities;

namespace Exchanger.API.Repositories.IRepositories
{
    public interface IListingRepository
    {
        Task<bool> AddAsync(
            Listing listing, 
            List<ListingImages> images, 
            List<ListingCategory> categories);
        Task<List<DisplayListingDTO>> GetListingInfoByUserIdAsync(
            Guid userId,
            Guid? lastListingId,
            int limit);
        Task<List<DisplayListingDTO>> GetListingByParamsAsync(ListingParams listingParams);
        Task<List<DisplayListingDTO>> SearchByTitleAsync(string title);
        Task<Listing?> GetListingByIdAsync(Guid listingId);
        Task<DisplayListingDTO?> GetListingInfoByIdAsync(Guid listingId);
        Task<bool> AddImageAsync(List<ListingImages> images);
        Task<bool> DeleteImageAsync(ListingImages image);
        Task<bool> AddCategoryAsync(List<Category> categories);
        Task<bool> DeleteCategoryAsync(Guid listingId, int categoryId);
        Task<bool> UpdateListingAsync(Listing listing);
        Task<bool> DeleteListingAsync(Listing listing);
        Task<List<Category>> GetAllCategoriesAsync();
    }
}
