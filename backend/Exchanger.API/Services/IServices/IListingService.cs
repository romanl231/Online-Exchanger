using Exchanger.API.DTOs.ListingDTOs;
using Exchanger.API.Entities;
using Exchanger.API.Enums.ListingErrors;

namespace Exchanger.API.Services.IServices
{
    public interface IListingService
    {
        Task<ListingResult> CreateNewListingAsync(
            ListingCreationDTO listingCreationDTO, 
            Guid userId);
        Task<ListingResult> GetListingInfoByUserIdAsync(
            Guid userId, 
            Guid? lastListingId, 
            int limit);
        Task<ListingResult> GetListingByParamsAsync(
            ListingParams listingParams, 
            Guid? lastListingId, 
            int limit);
        Task<ListingResult> SearchByTitleAsync(
            string title,
            Guid? lastListingId,
            int limit);
        Task<ListingResult> DeactivateListingAsync(
            Guid listingId);
        Task<ListingResult> ActivateListingAsync(
            Guid listingId);
        Task<ListingResult> DeleteListingAsync(
            Guid listingId,
            Guid userId);
        Task<List<Category>> GetAllCategoriesAsync();
    }

    public interface IListingImageService
    {
        Task<ListingResult> AddImageAsync(
            List<IFormFile> images,
            Guid listingId,
            Guid userId);
        Task<ListingResult> DeleteImageAsync(
            Guid userId,
            Guid listingId,
            string avatarUrl);
    }

    public interface IListingCategoryService
    {
        Task<ListingResult> AddCategorysAsync(
            Guid userId,
            Guid listingId,
            int[] categoryIds);
        Task<ListingResult> DeleteCategoryAsync(
            Guid listingId,
            Guid userId,
            int categoryId);
    }
}
