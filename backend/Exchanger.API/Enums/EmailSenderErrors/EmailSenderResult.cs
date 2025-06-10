using CloudinaryDotNet.Actions;

namespace Exchanger.API.Enums.UploadToCloudErrors
{
    public class EmailSenderResult
    {
        public bool IsSuccess { get; init; }
        public EmailSenderErrorCode? ErrorCode { get; init; }

        public static EmailSenderResult Success() => new() 
        { 
            IsSuccess = true,  
        };

        public static EmailSenderResult Fail(EmailSenderErrorCode error) => new() 
        { 
            IsSuccess = false, 
            ErrorCode = error 
        };
    }
}
