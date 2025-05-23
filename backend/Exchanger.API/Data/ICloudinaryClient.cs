using CloudinaryDotNet.Actions;

namespace Exchanger.API.Data
{
    public interface ICloudinaryClient
    {
        Task<ImageUploadResult> UploadAsync(
            ImageUploadParams parameters,
            CancellationToken cancellationToken = default);
    }
}
