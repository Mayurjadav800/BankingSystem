using AutoMapper;
using BankingSystem.Dto;
using BankingSystem.Model;

namespace BankingSystem.Mapper
{
    public class MapperConfiguration :Profile
    {
        public MapperConfiguration()
        {
             CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<Deposite, DepositeDto>().ReverseMap();
            CreateMap<Account, DepositeDto>().ReverseMap();
            CreateMap<Withdraw,WithdrawDto>().ReverseMap();
            CreateMap<Transfer, TransferDto>().ReverseMap();
            CreateMap<Otp,OtpDto>().ReverseMap();


            CreateMap<Account,LogginDto>().ReverseMap();
        }
    }
}
