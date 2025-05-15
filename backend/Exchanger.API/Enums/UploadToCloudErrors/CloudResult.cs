using CloudinaryDotNet.Actions;
using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Entities;
using Exchanger.API.Enums.AuthErrors;

namespace Exchanger.API.Enums.UploadToCloudErrors
{
    public class CloudResult
    {
        public bool IsSuccess { get; init; }
        public ImageUploadResult? UploadResult { get; init; }
        public CloudErrorCode? ErrorCode { get; init; }

        public static CloudResult Success(ImageUploadResult uploadResult) => new() 
        { 
            IsSuccess = true, 
            UploadResult = uploadResult, 
        };

        public static CloudResult Success() => new()
        {
            IsSuccess = true,
        };

        public static CloudResult Fail(CloudErrorCode error) => new() 
        { 
            IsSuccess = false, 
            ErrorCode = error 
        };
    }
}
