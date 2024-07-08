using AutoMapper;
using BankingSystem.Data;
using BankingSystem.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using PaymentMicroServices.Data;
using PaymentMicroServices.Dto;
using PaymentMicroServices.Model;

namespace PaymentMicroServices.Repository
{
    public class CompundRepository : ICompundRepository
    {
        private readonly PaymentDbContext _paymentDbContext;
        private readonly IMapper _mapper;

        public CompundRepository(PaymentDbContext paymentDbContext,IMapper mapper)
        {
            _paymentDbContext = paymentDbContext;
            _mapper = mapper;
        }
        public async Task<CompundDto> CreateCompund(CompundDto compundDto)
        {
            if(compundDto == null)
            {
                throw new ArithmeticException("Id already existing");
            }
            if (compundDto.Principal < 0)
            {
                throw new Exception("Current Balance is Not negative");
            }
            var compund = _mapper.Map<Compund>(compundDto);
            var amount = compund.Principal *(decimal) Math.Pow((1 + compund.Rate / compund.CompoundingsPerYear),
                        (compund.CompoundingsPerYear * compund.Time));
            //Compound Interest = Principal amount (1 + InterestRate/Years) ^ (Years * number of times).
            var interest = amount - compund.Principal;
            compund.Interest = Math.Round(interest, 2);
             _paymentDbContext.Add(compund);
             await _paymentDbContext.SaveChangesAsync();
            return _mapper.Map<CompundDto>(compund);
        }

        public async Task<List<CompundDto>> GetAll()
        {
            var compund = await _paymentDbContext.Compund.ToListAsync();
            return _mapper.Map<List<CompundDto>>(compund);
        }
    }
}


