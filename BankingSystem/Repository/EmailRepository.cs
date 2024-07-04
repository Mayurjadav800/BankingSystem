using BankingSystem.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace BankingSystem.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly EmailSetting _emailSetting;

        public EmailRepository(IOptions<EmailSetting> options)
        {
            _emailSetting = options.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailSetting.Email);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder
            {
                HtmlBody = mailRequest.Body
            };
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
             smtp.Connect(_emailSetting.Host, _emailSetting.Port, SecureSocketOptions.StartTls);
             smtp.Authenticate(_emailSetting.Email, _emailSetting.Password);
            await smtp.SendAsync(email);
             smtp.Disconnect(true);
        }
    }
}


