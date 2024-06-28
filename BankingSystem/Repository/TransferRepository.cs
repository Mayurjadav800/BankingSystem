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
        private readonly IEmailRepository _emailRepository;

        public TransferRepository(IMapper mapper, AccountDbContext accountDbContext,IEmailRepository emailRepository)
        {
            _mapper = mapper;
            _accountDbContext = accountDbContext;
            _emailRepository = emailRepository;
        }
        public async Task<TransferDto> CreateTransfer(TransferDto transferDto)
        {
            if (transferDto.TransferAmount <= 0)
            {
                throw new ArgumentException("Transfer amount must be greater than zero");
            }
            if(transferDto.TransferAmount <= 0)
            {
                throw new ArgumentException("Receiver amount must be greater than zero");
            }
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
                    transfer.AccountId = account1.Id;
                   
                   await _accountDbContext.Transfer.AddAsync(transfer);
                    //_accountDbContext.SaveChanges();
                    await _accountDbContext.SaveChangesAsync();
                     transaction.Commit();
                    var mailRequest = new MailRequest()
                    {
                        ToEmail = account1.Email,
                        Subject = "Transfer Confirmation",
                        Body = $"{account1.FirstName}, you have successfully transferred {transferDto.TransferAmount} to {account2.FirstName}. Your new balance is {account1.CurrentBalance}."
                    };
                    await _emailRepository.SendEmailAsync(mailRequest);

                    var mailRequest1 = new MailRequest()
                    {
                        ToEmail = account2.Email,
                        Subject = "Transfer Confirmation",
                        Body = $"{account2.FirstName}, you have received {transferDto.TransferAmount} from {account1.FirstName}. Your new balance is {account2.CurrentBalance}."
                    };
                    await _emailRepository.SendEmailAsync(mailRequest1);



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


