namespace Exchanger.API.Services.IServices
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageToCloudAsync(IFormFile file, Guid userId);
    }
}
