using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Exchanger.API.Data
{
    public class CloudinaryClient : ICloudinaryClient
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryClient(Cloudinary cloudinary) 
        {
            _cloudinary = cloudinary;
        }

        public async Task<ImageUploadResult> UploadAsync(
            ImageUploadParams parameters,
            CancellationToken cancellationToken = default)
        {
            var result = await _cloudinary.UploadAsync(parameters, cancellationToken);
            return result;
        }
    }
}
