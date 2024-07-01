using BankingSystem.Dto;
using BankingSystem.Model;

namespace BankingSystem.Repository
{
    public interface IAuthenticationRepository
    {
        Task<string> CreateAuthentication(LogginDto logginDto);
    }
}

