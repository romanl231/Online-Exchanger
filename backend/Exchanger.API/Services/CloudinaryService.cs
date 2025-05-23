using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Exchanger.API.Data;
using Exchanger.API.Enums.UploadToCloudErrors;
using Exchanger.API.Services.IServices;

namespace Exchanger.API.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly ICloudinaryClient _client;
        private readonly int _maxSizeInBytes = 5 * 1024 * 1024;
        private readonly string[] _allowedTypes =
        {
            "image/jpeg",
            "image/png",
            "image/webp",
            "image/jpg",        
            "image/gif",        
            "image/bmp",        
            "image/tiff",       
            "image/svg+xml",    
            "image/heif",       
            "image/heic"   
        };

        public CloudinaryService(ICloudinaryClient client) 
        {
            _client = client;
        }

        public async Task<CloudResult> UploadAvatarToCloudAsync(
            IFormFile image, 
            Guid userId)
        {
            var result = await UploadSingleAsync(
                image, 
                userId.ToString(), 
                "exchanger_profile_pics");
            return result;
        }

        public async Task<CloudResult[]> UploadListingImagesToCloudAsync(
            List<IFormFile> images, 
            Guid userId, 
            Guid listingId)
        {
            var validationResult = ValidateImages(images);

            if (validationResult != null)
                return [ validationResult ];

            var identifier = $"{listingId}_{userId}";

            var uploadTasks = images.Select(image => UploadSingleAsync(
                image, 
                identifier, 
                "exchanger_listing_images"));

            var results = await Task.WhenAll(uploadTasks);

            if (results.Any(r => r.IsSuccess == false))
                throw new InvalidOperationException("Something went wrong");

            return results;
        }

        public async Task<CloudResult> UploadSingleAsync(
            IFormFile image, 
            string identefier, 
            string folder)
        {
            var validationResult = ValidateImage(image);

            if (validationResult != null)
                return validationResult;

            using var stream = image.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(CreateFileName(image, identefier), stream),
                Folder = folder,
            };
            var uploadResult = await _client.UploadAsync(uploadParams);

            return CloudResult.Success(uploadResult);
        }
        
        public string CreateFileName(
            IFormFile image, 
            string identefier)
        {
            var dateTime = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var fileName = $"{identefier}_{dateTime}{image.FileName}";
            return fileName;
        }

        public CloudResult? ValidateImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                throw new ArgumentNullException(nameof(image));
            }

            if (image.Length > _maxSizeInBytes)
            {
                return CloudResult.Fail(CloudErrorCode.FileSize);
            }

            if (!_allowedTypes.Contains(image.ContentType))
            {
                return CloudResult.Fail(CloudErrorCode.FileFormat);
            }

            return null;
        }

        public CloudResult? ValidateImages(List<IFormFile> images)
        {
            foreach (var image in images)
            {
                if (image == null || image.Length == 0)
                {
                    throw new ArgumentNullException(nameof(image));
                }

                if (image.Length > _maxSizeInBytes)
                {
                    return CloudResult.Fail(CloudErrorCode.FileSize);
                }

                if (!_allowedTypes.Contains(image.ContentType))
                {
                    return CloudResult.Fail(CloudErrorCode.FileFormat);
                }
            }

            return null;
        }
    }
}
