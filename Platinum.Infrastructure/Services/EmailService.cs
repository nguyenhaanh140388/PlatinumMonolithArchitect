using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Platinum.Core.Abstractions.Dtos;
using Platinum.Core.Abstractions.Services;
using Platinum.Core.Exceptions;
using Platinum.Core.Settings;
using Serilog;
using System.Threading.Tasks;


namespace Platinum.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public MailSettings _mailSettings { get; }
        public ILogger _logger { get; }

        public EmailService(IOptions<MailSettings> mailSettings, ILogger logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                // create message
                var email = new MimeMessage
                {
                    Sender = new MailboxAddress(_mailSettings.DisplayName, request.From ?? _mailSettings.EmailFrom)
                };
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                var builder = new BodyBuilder
                {
                    HtmlBody = request.Body
                };
                email.Body = builder.ToMessageBody();

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                    await smtp.SendAsync(email);
                    smtp.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw new ApiException(ex.Message);
            }
        }
    }
}
