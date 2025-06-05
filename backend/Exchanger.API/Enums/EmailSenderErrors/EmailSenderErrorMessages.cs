using Exchanger.API.Enums.AuthErrors;

namespace Exchanger.API.Enums.UploadToCloudErrors
{
    public static class EmailSenderErrorMessages
    {
        public static readonly Dictionary<EmailSenderErrorCode, string> Messages = new()
        {
            { EmailSenderErrorCode.UnknownError, "An unknown error occurred while sending the email." },
            { EmailSenderErrorCode.SmtpConnectionFailed, "Could not connect to the email server." },
            { EmailSenderErrorCode.AuthenticationFailed, "Failed to authenticate with the email server." },
            { EmailSenderErrorCode.InvalidRecipient, "The specified recipient email address is invalid." },
            { EmailSenderErrorCode.MessageNotSent, "The email message could not be sent." },
            { EmailSenderErrorCode.TemplateNotFound, "The email template was not found." },
            { EmailSenderErrorCode.TemplateRenderFailed, "Failed to render the email template." },
            { EmailSenderErrorCode.TokenGenerationFailed, "Failed to generate a verification token." },
            { EmailSenderErrorCode.EmailDisabledInConfig, "Email sending is currently disabled in the configuration." },
            { EmailSenderErrorCode.AttachmentMissing, "One or more attachments were missing or invalid." },
            { EmailSenderErrorCode.InvalidEmailFormat, "The email address format is not valid." }
        };
    }
}
