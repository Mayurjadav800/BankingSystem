using BankingSystem.Dto;
using PaymentMicroServices.Dto;

namespace PaymentMicroServices.Repository
{
    public interface ICompundRepository
    {
        Task<List<CompundDto>> GetAll();
        Task<CompundDto> CreateCompund(CompundDto compundDto);
    }
}
