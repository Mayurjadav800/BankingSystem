using BankingSystem.Dto;

namespace BankingSystem.Repository
{
    public interface IWithdrawRepository
    {
        Task<WithdrawDto> CreateWithdraw(WithdrawDto withdrawDto);
    }
}
