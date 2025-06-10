using Exchanger.API.DTOs;
using Exchanger.API.DTOs.EmailSenderDTOs;
using Exchanger.API.Enums.UploadToCloudErrors;
using Exchanger.API.Services.IServices;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Exchanger.API.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly string _appBaseUrl;

        public EmailSenderService(
            SmtpSettings smtpSettings, 
            IOptions<AppSettings> appSettings) 
        {
            _smtpSettings = smtpSettings;
            _appBaseUrl = appSettings.Value.BaseUrl;
        }

        public async Task<EmailSenderResult> SendVerificationEmailAsync(string email, string verificationToken)
        {
            try
            {
                var body = await GetVerificationEmailBodyAsync(verificationToken);
                await SendEmailAsync(email, "Email Verification", body);
                return EmailSenderResult.Success();
            }
            catch (FileNotFoundException)
            {
                return EmailSenderResult.Fail(EmailSenderErrorCode.TemplateNotFound);
            }
            catch (SmtpException)
            {
                return EmailSenderResult.Fail(EmailSenderErrorCode.SmtpConnectionFailed);
            }
            catch (Exception)
            {
                return EmailSenderResult.Fail(EmailSenderErrorCode.UnknownError);
            }
        }

        public async Task<string> GetVerificationEmailBodyAsync(string token)
        {
            var templatePath = Path.Combine("Templates", "Emails", "VerifyEmail.html");
            var htmlTemplate = await File.ReadAllTextAsync(templatePath);

            var verificationUrl = $"{_appBaseUrl}/verify?token={token}";
            var persolnalizredHtml = htmlTemplate.Replace("{{verification_link}}", verificationUrl);

            return persolnalizredHtml;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = _smtpSettings.EnableSsl,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.Username),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }
    }
}
