using Exchanger.API.DTOs.ListingDTOs;
using Exchanger.API.Entities;
using Exchanger.API.Enums.ListingErrors;
using Exchanger.API.Repositories.IRepositories;
using Exchanger.API.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Exchanger.API.Services
{
    public class ListingService : IListingService
    {
        private readonly IListingRepository _listingRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private const int maxLimit = 15; //if input limit = 0, limit = maxLimit 

        public ListingService(
            IListingRepository listingRepository, 
            ICloudinaryService cloudinaryService)
        {
            _listingRepository = listingRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ListingResult> CreateNewListingAsync(ListingCreationDTO dto, Guid userId)
        {
            if (userId == Guid.Empty)
                return ListingResult.Fail(ListingErrorCode.InvalidUserId);

            var validationResult = EnsureCreationDTOValid(dto);
            if (!validationResult.IsSuccess)
                return validationResult;

            var (listing, images, categories) = await BuildListingFromDTO(dto, userId);

            var saved = await _listingRepository.AddAsync(listing, images, categories);

            return saved
                ? ListingResult.Success()
                : ListingResult.Fail(ListingErrorCode.UnknownError);
        }


        public async Task<(
            Listing listing,  
            List<ListingImages> listingImages,
            List<ListingCategory> listingCategories)> BuildListingFromDTO(ListingCreationDTO listingCreationDTO, Guid userId)
        {
            var listing = new Listing
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = listingCreationDTO.Title,
                Description = listingCreationDTO.Description,
                Price = listingCreationDTO.Price,
                IsActive = true,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
            };

            var listingCategories = listingCreationDTO.CategoryIds.Select(cId => new ListingCategory
            {
                CategoryId = cId,
                ListingId = listing.Id,
            }).ToList();
        
            var cloudinaryResult = await _cloudinaryService.UploadListingImagesToCloudAsync(
                listingCreationDTO.Images,
                userId,
                listing.Id);

            var listingImages = cloudinaryResult
                .Where(cr => cr.IsSuccess)
                .Select(cr => new ListingImages
                 {
                    Id = Guid.NewGuid(),
                    ListingId = listing.Id,
                    ImageUrl = cr.UploadResult.SecureUrl.AbsoluteUri
               }).ToList();

            return (listing, listingImages, listingCategories);
        }

        public ListingResult EnsureCreationDTOValid(ListingCreationDTO listingCreationDTO)
        {
            if (listingCreationDTO == null)
                return ListingResult.Fail(ListingErrorCode.NullDto);

            if (!listingCreationDTO.CategoryIds.Any())
                return ListingResult.Fail(ListingErrorCode.NoCategories);

            if (listingCreationDTO.CategoryIds.Count() > maxLimit)
                return ListingResult.Fail(ListingErrorCode.TooManyCategories);

            if (!listingCreationDTO.Images.Any())
                return ListingResult.Fail(ListingErrorCode.NoImages);

            if (listingCreationDTO.Images.Count() > maxLimit)
                return ListingResult.Fail(ListingErrorCode.TooManyImages);

            return ListingResult.Success();
        }

        public async Task<ListingResult> GetListingInfoByUserIdAsync(
            Guid userId,
            Guid? lastListingId,
            int limit)
        { 
            if(userId == Guid.Empty) 
                return ListingResult.Fail(ListingErrorCode.InvalidUserId);

            if (limit == 0) { limit = NormalizeLimit(limit); }

            var response = await _listingRepository.GetListingInfoByUserIdAsync(
                userId, 
                lastListingId, 
                limit);

            return ListingResult.Success(response);
        }

        public async Task<ListingResult> GetListingByParamsAsync(
            ListingParams listingParams)
        {
            if (listingParams == null)
                return ListingResult.Fail(ListingErrorCode.InvalidParams);

            if (listingParams.CategoryIds.Count == 0)
            {
                var categories = await GetAllCategoriesAsync();
                listingParams.CategoryIds = categories.Select(c => c.Id).ToList();
            }

            var response = await _listingRepository.GetListingByParamsAsync(listingParams);

            return ListingResult.Success(response);
        }

        public async Task<ListingResult> SearchByTitleAsync(
            string title,
            Guid? lastListingId,
            int limit)
        {
            if(string.IsNullOrEmpty(title))
                return ListingResult.Fail(ListingErrorCode.InvalidTitle);

            if (limit == 0) { limit = NormalizeLimit(limit); }

            var result = await _listingRepository.SearchByTitleAsync(
                title, 
                lastListingId, 
                limit);

            return ListingResult.Success(result);
        }

        public async Task<ListingResult> DeactivateListingAsync(
            Guid listingId,
            Guid userId)
        {
            var listing = await _listingRepository.GetListingByIdAsync(listingId);

            if (listing == null)
                return ListingResult.Fail(ListingErrorCode.ListingNotFound);

            if(listing.UserId != userId)
                return ListingResult.Fail(ListingErrorCode.UnauthorizedAccess);

            listing.Updated = DateTime.UtcNow;
            listing.IsActive = false;
            await _listingRepository.UpdateListingAsync(listing);
            return ListingResult.Success();
        }

        public async Task<ListingResult> ActivateListingAsync(
            Guid listingId,
            Guid userId)
        {
            var listing = await _listingRepository.GetListingByIdAsync(listingId);

            if (listing == null)
                return ListingResult.Fail(ListingErrorCode.ListingNotFound);

            if (listing.UserId != userId)
                return ListingResult.Fail(ListingErrorCode.UnauthorizedAccess);

            listing.Updated = DateTime.UtcNow;
            listing.IsActive = true;
            await _listingRepository.UpdateListingAsync(listing);
            return ListingResult.Success();
        }

        public async Task<ListingResult> DeleteListingAsync(
            Guid listingId,
            Guid userId)
        {
            var listing = await _listingRepository.GetListingByIdAsync(listingId);

            if (listing == null)
                return ListingResult.Fail(ListingErrorCode.ListingNotFound);

            if (listing.UserId != userId)
                return ListingResult.Fail(ListingErrorCode.UnauthorizedAccess);

            await _listingRepository.DeleteListingAsync(listing);
            return ListingResult.Success();
        }

        private async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _listingRepository.GetAllCategoriesAsync();
        }

        private int NormalizeLimit(int limit) => limit == 0 ? maxLimit : limit;
    }
}
