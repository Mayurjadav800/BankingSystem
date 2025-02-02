﻿using AutoMapper;
using BankingSystem.Data;
using BankingSystem.Dto;
using BankingSystem.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BankingSystem.Repository
{
    public class DepositeRepository : IDepositeRepository
    {
        private readonly IMapper _mapper;
        private readonly AccountDbContext _accountDbContext;
        private readonly IEmailRepository _emailRepository;

        public DepositeRepository(IMapper mapper, AccountDbContext accountDbContext,IEmailRepository emailRepository)
        {
            _mapper = mapper;
            _accountDbContext = accountDbContext;
            _emailRepository = emailRepository;
        }
        public async Task<DepositeDto> CreateDepository(DepositeDto depositeDto)
        {
            if (depositeDto.DepositeAmount <= 0)
            {
                throw new ArgumentException("Deposite amount must be greater than zero");
            }
            using (var transaction = await _accountDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var account = await _accountDbContext.Account
                        .FirstOrDefaultAsync(e => e.Id == depositeDto.AccountId);

                    if (account == null)
                    {
                        throw new Exception("Account not found");
                    }
                    var deposite = _mapper.Map<Deposite>(depositeDto);
                   // deposite.CreatedAt = DateTime.UtcNow;
                    //deposite.AccountId = account.Id;
                    await _accountDbContext.Deposite.AddAsync(deposite);
                    await _accountDbContext.SaveChangesAsync();
                    var users = _mapper.Map<DepositeDto>(deposite);
                    account.CurrentBalance = account.CurrentBalance + depositeDto.DepositeAmount;
                   
                    _accountDbContext.Account.Update(account);
                    await _accountDbContext.SaveChangesAsync();
                    transaction.Commit();
                    var mailRequest = new MailRequest()
                    {
                        ToEmail = account.Email,
                        Subject = "Deposite Confirmation",
                        Body = $"{account.FirstName},your deposite of {depositeDto.DepositeAmount} has succesfully processed."

                    };
                    await _emailRepository.SendEmailAsync(mailRequest);

                    return _mapper.Map<DepositeDto>(depositeDto);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
