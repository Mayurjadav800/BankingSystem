using BankingSystem.Data;
using BankingSystem.Model;
using Microsoft.EntityFrameworkCore;
using Quartz;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace BankingSystem.Repository
{
    public class BirthDayEmailJob : IJob
    {
        private readonly IEmailRepository _emailRepository;
        private readonly AccountDbContext _accountDbContext;

        public BirthDayEmailJob(IEmailRepository emailRepository,AccountDbContext accountDbContext)
        {
            _emailRepository = emailRepository;
            _accountDbContext = accountDbContext;
        }
        public async Task Execute(IJobExecutionContext context)
        {
                // often email to the user
            DateTime today = DateTime.Today;
            var accountbirthday = await _accountDbContext.Account.Where(a => a.DateOfBirth.Day == today.Day).ToListAsync();
            foreach (var account in accountbirthday)
            {
                await SendBirthdayEmailAsync(account);
            }
        }
        private async Task SendBirthdayEmailAsync(Account account)
        {
            var mailRequest = new MailRequest
            {
                ToEmail = account.Email,
                Subject = "Happy Birthday!",
                Body = $"Dear {account.FirstName},\n \n \n Happy Birthday!"
            };
            await _emailRepository.SendEmailAsync(mailRequest);
        }
    }
}
