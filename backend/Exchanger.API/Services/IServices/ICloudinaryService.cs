using Exchanger.API.Enums.UploadToCloudErrors;

namespace Exchanger.API.Services.IServices
{
    public interface ICloudinaryService
    {
        Task<CloudResult> UploadImageToCloudAsync(IFormFile file, Guid userId);
    }
}
