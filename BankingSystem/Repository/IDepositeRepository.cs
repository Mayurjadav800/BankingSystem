using BankingSystem.Dto;

namespace BankingSystem.Repository
{
    public interface IDepositeRepository
    {
        Task<DepositeDto> CreateDepository(DepositeDto depositeDto);
    }
}
