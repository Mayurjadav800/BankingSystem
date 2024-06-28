using BankingSystem.Model;

namespace BankingSystem.Repository
{
    public interface IEmailRepository
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
