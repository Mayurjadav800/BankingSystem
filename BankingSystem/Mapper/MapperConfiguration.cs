﻿using AutoMapper;
using BankingSystem.Dto;
using BankingSystem.Model;

namespace BankingSystem.Mapper
{
    public class MapperConfiguration :Profile
    {
        public MapperConfiguration()
        {
            //CreateMap<AccountDto, Account>()
            //.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName ?? "")); // Map LastName, handling nulls
            //CreateMap<Account, AccountDto>();
             CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<Deposite, DepositeDto>().ReverseMap();
            CreateMap<Account, DepositeDto>().ReverseMap();
            CreateMap<Withdraw,WithdrawDto>().ReverseMap();
            CreateMap<Transfer, TransferDto>().ReverseMap();
        }
    }
}