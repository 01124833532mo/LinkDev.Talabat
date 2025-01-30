using LinkDev.Talabat.Core.Application.Abstraction.Models._Common;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Emails;
using LinkDev.Talabat.Shared.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace LinkDev.Talabat.Core.Application.Services.Emails
{
    public class EmailSettings(IOptions<mailSettings> MailSetting) : IEmailSettings
    {
        private readonly mailSettings mailSettings = MailSetting.Value;
        public async Task SendEmail(Email emailDto)
        {

            var Email = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(mailSettings.Email),
                Subject = emailDto.Subject
            };

            Email.To.Add(MailboxAddress.Parse(emailDto.To));
            Email.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Email));


            var EmailBody = new BodyBuilder();
            EmailBody.TextBody = emailDto.Body;


            Email.Body = EmailBody.ToMessageBody();


            using var Smtp = new SmtpClient();

            Smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

            await Smtp.ConnectAsync(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);


            await Smtp.AuthenticateAsync(mailSettings.Email, mailSettings.Password);


            await Smtp.SendAsync(Email);


            await Smtp.DisconnectAsync(true);
        }
    }
}
