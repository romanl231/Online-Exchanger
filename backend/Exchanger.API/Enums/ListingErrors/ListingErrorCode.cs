namespace Exchanger.API.Enums.ListingErrors
{
    public enum ListingErrorCode
    {
        UnknownError,

        // DTO validation errors
        NullDto,
        NoCategories,
        TooManyCategories,
        NoImages,
        TooManyImages,

        // Entity-related errors
        InvalidUserId,
        ListingNotFound,
        UnauthorizedAccess,
        InvalidTitle,
        InvalidParams,
    }
}
