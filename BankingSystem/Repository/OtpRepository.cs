using BankingSystem.Data;
using BankingSystem.Dto;
using BankingSystem.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BankingSystem.Repository
{
    public class OtpRepository : IOtpRepository
    {
        private readonly AccountDbContext _accountDbContext;
        private readonly IEmailRepository _emailRepository;

        public OtpRepository(AccountDbContext accountDbContext,IEmailRepository emailRepository)
        {
            _accountDbContext = accountDbContext;
            _emailRepository = emailRepository;
        }
      

        public async Task<string> GenerateOtp(int accountId)
        {
            try
            {
                var account = await _accountDbContext.Account.Where(e=>e.AccountId == accountId).FirstOrDefaultAsync();
              
                if (account == null)
                {
                    throw new ArgumentException($"Account with ID {accountId} does not exist.");
                }
                var existingOtp = await _accountDbContext.Otp
                .Where(e => e.AccountId == accountId && e.ExpiryDate > DateTime.UtcNow)
                .FirstOrDefaultAsync();
                if (existingOtp != null)
                {
                    return existingOtp.Code;
                }
                var otpCode = GenerateRandomOtp();
                var otp = new Otp
                {
                    AccountId = account.AccountId,
                    Code = otpCode,
                    ExpiryDate = DateTime.UtcNow.AddMinutes(2),
                    IsUsed = false
                };
                //otp.IsUsed = true;
                _accountDbContext.Otp.Add(otp);
                await _accountDbContext.SaveChangesAsync();

                //var account = await _accountDbContext.Account.FindAsync(accountId);
                if (account != null)
                {
                    var mailRequest = new MailRequest()
                    {
                        ToEmail = account.Email,
                        Subject = "OTP for Transfer Confirmation",
                        Body = $"Your OTP for transfer is: {otpCode}"
                    };
                    await _emailRepository.SendEmailAsync(mailRequest);
                }

                return otp.Code;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        private string GenerateRandomOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public async Task<bool> VerifyOtp(int accountId, string otpCode)
        {
            try
            {
                var account = await _accountDbContext.Account.Where(e => e.AccountId == accountId).FirstOrDefaultAsync();

                if (account == null)
                {
                    throw new ArgumentException($"Account with ID {accountId} does not exist.");
                }

                //var otp = await _accountDbContext.Otp
                //    .Where(e => e.AccountId == account.Id && e.Code == otpCode && e.ExpiryDate > DateTime.UtcNow)
                //    .FirstOrDefaultAsync();

                var otp = await _accountDbContext.Otp
                .Where(e => e.AccountId == account.Id && e.Code == otpCode && e.ExpiryDate > DateTime.UtcNow)
                .FirstOrDefaultAsync();

                if (otp != null)
                {
                    otp.IsUsed = true;
                    _accountDbContext.Otp.Update(otp);
                    await _accountDbContext.SaveChangesAsync();
                    return true; 
                }

                return false;
            }
            catch (Exception ex)
            {
               
                throw new Exception("Error verifying OTP", ex);
            }
        }


    }
}
