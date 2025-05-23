using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.DTOs.ListingDTOs;
using Exchanger.API.Entities;

namespace Exchanger.API.Enums.ListingErrors
{
    public class ListingResult
    {
        public bool IsSuccess { get; init; }
        public DisplayListingDTO? Listing { get; init; }
        public List<DisplayListingDTO>? Listings { get; init; }
        public Listing? ListingEntity { get; init; }
        public ListingErrorCode? ErrorCode { get; init; }

        public static ListingResult Success(Listing listingEntity) => new()
        {
            IsSuccess = true,
            ListingEntity = listingEntity
        };

        public static ListingResult Success(DisplayListingDTO listing) => new() 
        { 
            IsSuccess = true, 
            Listing = listing 
        };

        public static ListingResult Success(List<DisplayListingDTO> listings) => new()
        {
            IsSuccess = true,
            Listings = listings
        };

        public static ListingResult Success() => new() 
        { 
            IsSuccess = true
        };

        public static ListingResult Fail(ListingErrorCode error) => new() 
        { 
            IsSuccess = false, 
            ErrorCode = error 
        };

        public static ListingResult Fail() => new()
        {
            IsSuccess = false,
        };
    }
}
