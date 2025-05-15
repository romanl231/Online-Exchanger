using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Exchanger.API.Enums.UploadToCloudErrors;
using Exchanger.API.Services.IServices;

namespace Exchanger.API.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
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
        public CloudinaryService(IConfiguration configuration) 
        {
            var account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]
            );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<CloudResult> UploadImageToCloudAsync(IFormFile image, Guid userId)
        {
            var validationResult = ValidateImage(image);

            if (validationResult != null)
                return validationResult;

            using var stream = image.OpenReadStream();
            var uploadParams =  new ImageUploadParams
            {
                File = new FileDescription(CreateFileName(image, userId), stream),
                Folder = "exchanger_profile_pics",
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return CloudResult.Success(uploadResult);
        }

        public string CreateFileName(IFormFile image, Guid userId)
        {
            var dateTime = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var fileName = $"{userId}_{dateTime}{image.FileName}";
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
    }
}
