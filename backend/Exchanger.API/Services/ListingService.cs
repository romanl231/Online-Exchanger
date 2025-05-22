using Exchanger.API.DTOs.ListingDTOs;
using Exchanger.API.Entities;
using Exchanger.API.Enums.ListingErrors;
using Exchanger.API.Repositories.IRepositories;
using Exchanger.API.Services.IServices;

namespace Exchanger.API.Services
{
    public class ListingService : IListingService
    {
        private readonly IListingRepository _listingRepository;
        private readonly ICloudinaryService _cloudinaryService;

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
                throw new ArgumentNullException(nameof(userId));

            var validationResult = EnsureCreationDTOValid(dto);
            if (!validationResult.IsSuccess)
                return validationResult;

            var (listing, images, categories) = await BuildListingFromDTO(dto, userId);

            var saved = await _listingRepository.AddAsync(listing, images, categories);

            return saved
                ? ListingResult.Success()
                : ListingResult.Fail();
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
                throw new ArgumentNullException("DTO can't be null");

            if (!listingCreationDTO.CategoryIds.Any())
                throw new ArgumentNullException("You can't create listing without category");

            if (listingCreationDTO.CategoryIds.Count() > 15)
                throw new InvalidCastException("You can't choose more than 15 categories");

            if (!listingCreationDTO.Images.Any())
                throw new ArgumentNullException("You can't create listing without images");

            if (listingCreationDTO.Images.Count() > 15)
                throw new InvalidCastException("You can't upload more than 15 images");

            return ListingResult.Success();
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
