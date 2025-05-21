using Exchanger.API.DTOs.ListingDTOs;
using Exchanger.API.Entities;
using Exchanger.API.Enums.ListingErrors;
using Exchanger.API.Repositories.IRepositories;
using Exchanger.API.Services.IServices;

namespace Exchanger.API.Repositories
{
    public class ListingService : IListingService
    {
        private readonly IListingRepository _listingRepository;

        public ListingService(IListingRepository listingRepository)
        {
            _listingRepository = listingRepository;
        }

        public async Task<ListingResult> CreateNewListingAsync(
            ListingCreationDTO listingCreationDTO,
            Guid userId)
        {
            return ListingResult.Fail();
        }

        public async Task<ListingResult> GetListingInfoByUserIdAsync(
            Guid userId,
            Guid? lastListingId,
            int limit)
        {
            return ListingResult.Fail();
        }

        public async Task<ListingResult> GetListingByParamsAsync(
            ListingParams listingParams,
            Guid? lastListingId,
            int limit)
        {
            return ListingResult.Fail();
        }

        public async Task<ListingResult> SearchByTitleAsync(
            string title,
            Guid? lastListingId,
            int limit)
        {
            return ListingResult.Fail();
        }

        public async Task<ListingResult> DeactivateListingAsync(
            Guid listingId)
        {
            return ListingResult.Fail();
        }

        public async Task<ListingResult> ActivateListingAsync(
            Guid listingId)
        {
            return ListingResult.Fail();
        }

        public async Task<ListingResult> DeleteListingAsync(
            Guid listingId,
            Guid userId)
        {
            return ListingResult.Fail();
        }

        public async Task<List<Category>> GetAllCategoriesAsync() 
        {
            return new List<Category>();
        }
    }
}
