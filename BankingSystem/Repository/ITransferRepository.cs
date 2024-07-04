using BankingSystem.Dto;

namespace BankingSystem.Repository
{
    public interface ITransferRepository
    {
       Task<object> CreateTransfer(TransferDto transferDto);
    }
}
