using Exchanger.API.Entities;
using Exchanger.API.Enums.ListingErrors;
using Exchanger.API.Repositories.IRepositories;
using Exchanger.API.Services.IServices;

namespace Exchanger.API.Services
{
    public class ListingImageService : IListingImageService
    {
        private readonly IListingRepository _listingRepository;
        private readonly ICloudinaryService _cloudinaryService;

        public ListingImageService(IListingRepository listingRepository, ICloudinaryService cloudinaryService) 
        {
            _listingRepository = listingRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ListingResult> AddImageAsync(
            List<IFormFile> images,
            Guid listingId,
            Guid userId)
        {
            var accessValidationResult = await CheckUserAccessAsync(userId, listingId);

            if (!accessValidationResult.IsSuccess)
            {
                return accessValidationResult;
            }

            var cloudResponse = await _cloudinaryService.UploadListingImagesToCloudAsync(
                images, 
                userId, 
                listingId);

            var listingImageEntitys = cloudResponse
                .Where(cr => cr.IsSuccess == true)
                .Select(cr => new ListingImages
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = cr.UploadResult.SecureUrl.AbsoluteUri,
                    ListingId = listingId,
                }).ToList();

            var result = await _listingRepository.AddImageAsync(listingImageEntitys);

            return result ?
                  ListingResult.Success() :
                  ListingResult.Fail(ListingErrorCode.UnknownError);
        }

        public async Task<ListingResult> DeleteImageAsync(
            Guid userId,
            Guid listingId,
            string avatarUrl)
        {
            var accessValidationResult = await CheckUserAccessAsync(userId, listingId);

            if (!accessValidationResult.IsSuccess)
            {
                return accessValidationResult;
            }

            var result = await _listingRepository.DeleteImageAsync(listingId, avatarUrl);

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
    }
}
