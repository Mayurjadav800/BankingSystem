using BankingSystem.Dto;

namespace BankingSystem.Repository
{
    public interface IAccountRepository
    {
        Task<List<AccountDto>> GetAllAccount();
        Task<AccountDto> GetAccountById(int Id);
        Task<AccountDto> CreateAccount(AccountDto accountDto);

    }
}
