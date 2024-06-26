using BankingSystem.Dto;

namespace BankingSystem.Repository
{
    public interface ITransferRepository
    {
        Task<TransferDto> CreateTransfer(TransferDto transferDto);
    }
}
