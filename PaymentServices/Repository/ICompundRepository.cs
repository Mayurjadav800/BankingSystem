using PaymentMicroServices.Dto;

namespace PaymentMicroServices.Repository
{
    public interface ICompundRepository
    {
        Task<CompundDto> CreateCompund(CompundDto compundDto);
    }
}
