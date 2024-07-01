using BankingSystem.Dto;
using System.Runtime.InteropServices;

namespace BankingSystem.Repository
{
    public interface IOtpRepository
    {
        Task<string> GenerateOtp(int accountId);
        Task<bool> VerifyOtp(int accountId, string Code);
    }
}
