using Exchanger.API.Entities;
using Exchanger.API.Enums.ListingErrors;
using Exchanger.API.Repositories.IRepositories;
using Exchanger.API.Services.IServices;

namespace Exchanger.API.Services
{
    public class ListingCategoryService : IListingCategoryService
    {
        private readonly IListingRepository _listingRepository;

        public ListingCategoryService(IListingRepository listingRepository) 
        {
            _listingRepository = listingRepository;
        }

        public async Task<ListingResult> AddCategorysAsync(Guid userId, Guid listingId, int[] categoryIds)
        {
            var accessResult = await CheckUserAccessAsync(userId, listingId);

            if (!accessResult.IsSuccess)
                return accessResult;

            var categoryEntities = categoryIds.Select(cId => new ListingCategory
            {
                CategoryId = cId,
                ListingId = listingId,
            } ).ToList();

            var result = await _listingRepository.AddCategoryAsync(categoryEntities);

            return result ? 
                  ListingResult.Success() : 
                  ListingResult.Fail(ListingErrorCode.UnknownError);
        }

        public async Task<ListingResult> DeleteCategoryAsync(Guid userId, Guid listingId, int categoryId)
        {
            var accessResult = await CheckUserAccessAsync(userId, listingId);

            if (!accessResult.IsSuccess)
                return accessResult;

            var result = await _listingRepository.DeleteCategoryAsync(listingId, categoryId);

            return result ?
                  ListingResult.Success() :
                  ListingResult.Fail(ListingErrorCode.UnknownError);
        }

        public async Task<ListingResult> CheckUserAccessAsync(Guid userId, Guid listingId)
        {
            var listing = await _listingRepository.GetListingByIdAsync(listingId);
            if (listing == null)
                return ListingResult.Fail(ListingErrorCode.ListingNotFound);


            if (listing.UserId == userId)
                return ListingResult.Fail(ListingErrorCode.UnauthorizedAccess);

            return ListingResult.Success(listing);
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _listingRepository.GetAllCategoriesAsync();
        }
    }
}
