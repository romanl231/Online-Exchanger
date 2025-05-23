namespace Exchanger.API.Enums.ListingErrors
{
    public static class ListingErrorMessages
    {
        public static readonly Dictionary<ListingErrorCode, string> Messages = new()
        {
            { ListingErrorCode.UnknownError, "An unknown error occurred." },
            { ListingErrorCode.NullDto, "The listing creation data is null." },
            { ListingErrorCode.NoCategories, "At least one category must be specified." },
            { ListingErrorCode.TooManyCategories, "No more than 15 categories are allowed." },
            { ListingErrorCode.NoImages, "At least one image must be uploaded." },
            { ListingErrorCode.TooManyImages, "No more than 15 images can be uploaded." },
            { ListingErrorCode.InvalidUserId, "The provided user ID is invalid." },
            { ListingErrorCode.ListingNotFound, "The listing with the specified ID was not found." },
            { ListingErrorCode.UnauthorizedAccess, "You are not authorized to perform this action." },
            { ListingErrorCode.InvalidTitle, "The title provided is invalid." },
            { ListingErrorCode.InvalidParams, "The listing search parameters are invalid." }
        };
    }
}
