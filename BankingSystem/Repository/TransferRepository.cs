using AutoMapper;
using BankingSystem.Data;
using BankingSystem.Dto;
using BankingSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Repository
{
    public class TransferRepository : ITransferRepository
    {
        private readonly IMapper _mapper;
        private readonly AccountDbContext _accountDbContext;

        public TransferRepository(IMapper mapper, AccountDbContext accountDbContext)
        {
            _mapper = mapper;
            _accountDbContext = accountDbContext;
        }
        public async Task<TransferDto> CreateTransfer(TransferDto transferDto)
        {
            using (var transaction = await _accountDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var account1 = await _accountDbContext.Account
                        .FirstOrDefaultAsync(e => e.AccountId == transferDto.SenderId);
                    if (account1 == null)
                    {
                        throw new Exception("Account not found");
                    }
                    var account2 = await _accountDbContext.Account
                        .FirstOrDefaultAsync(e => e.AccountId == transferDto.ReceiverId);
                    if(account2 == null)
                    {
                        throw new Exception("Account was Not Found");
                    }
                    if(account1.CurrentBalance < transferDto.TransferAmount)
                    {
                        throw new Exception("Insuffient Money");
                    }
                    account1.CurrentBalance = account1.CurrentBalance - transferDto.TransferAmount;
                     _accountDbContext.Account.Update(account1);
                     account2.CurrentBalance = account2.CurrentBalance + transferDto.TransferAmount;
                    _accountDbContext.Account.Update(account2);
                    var transfer = _mapper.Map<Transfer>(transferDto);
                    transfer.SenderId = account1.Id;
                    transfer.ReceiverId = account2.Id;
                    await _accountDbContext.Transfer.AddAsync(transfer);
                    await _accountDbContext.SaveChangesAsync();
                     transaction.Commit();
                    var transfers = _mapper.Map<TransferDto>(transfer);
                    return transfers;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            

        }
    } }


