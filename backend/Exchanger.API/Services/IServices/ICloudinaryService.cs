using Exchanger.API.Enums.UploadToCloudErrors;

namespace Exchanger.API.Services.IServices
{
    public interface ICloudinaryService
    {
        Task<CloudResult> UploadAvatarToCloudAsync(
            IFormFile file, 
            Guid userId);
        Task<CloudResult[]> UploadListingImagesToCloudAsync(
            List<IFormFile> files, 
            Guid userId, 
            Guid listingId);
    }
}
