namespace Exchanger.API.Enums.UploadToCloudErrors
{
    public enum EmailSenderErrorCode
    {
        UnknownError,
        SmtpConnectionFailed,
        AuthenticationFailed,
        InvalidRecipient,
        MessageNotSent,
        TemplateNotFound,
        TemplateRenderFailed,
        TokenGenerationFailed,
        EmailDisabledInConfig,
        AttachmentMissing,
        InvalidEmailFormat
    }
}
