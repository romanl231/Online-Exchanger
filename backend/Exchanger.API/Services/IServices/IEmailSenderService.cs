using Exchanger.API.Enums.UploadToCloudErrors;

namespace Exchanger.API.Services.IServices
{
    public interface IEmailSenderService
    {
        Task<EmailSenderResult> SendVerificationEmailAsync(string email, string verificationToken);   
    }
}
