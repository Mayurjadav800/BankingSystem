using AutoMapper;
using BankingSystem.Data;
using BankingSystem.Dto;
using BankingSystem.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace BankingSystem.Repository
{
    public class TransferRepository : ITransferRepository
    {
        private readonly IMapper _mapper;
        private readonly AccountDbContext _accountDbContext;
        private readonly IEmailRepository _emailRepository;
        private readonly IOtpRepository _otpRepository;

        public TransferRepository(IMapper mapper, AccountDbContext accountDbContext, IEmailRepository emailRepository, IOtpRepository otpRepository)
        {
            _mapper = mapper;
            _accountDbContext = accountDbContext;
            _emailRepository = emailRepository;
            _otpRepository = otpRepository;
        }
        public async Task<TransferDto> CreateTransfer(TransferDto transferDto)
        {
            
            var otpValidate = await _accountDbContext.Otp
                .Where(e => e.AccountId == transferDto.AccountId && e.IsUsed == false && e.ExpiryDate > DateTime.UtcNow)
                .FirstOrDefaultAsync();
            //otpValidate.AccountId = Account.Id;
           

            if (otpValidate == null)
            {
                throw new ArgumentException("OTP verification failed. Cannot proceed with transfer.");
            }
            if (transferDto.TransferAmount <= 0)
            {
                throw new ArgumentException("Transfer amount must be greater than zero");
            }
            if (transferDto.TransferAmount <= 0)
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
                    if (account2 == null)
                    {
                        throw new Exception("Account was Not Found");
                    }
                    if (account1.CurrentBalance < transferDto.TransferAmount)
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
    }
}








//using AutoMapper;
//using BankingSystem.Data;
//using BankingSystem.Dto;
//using BankingSystem.Model;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Threading.Tasks;

//namespace BankingSystem.Repository
//{
//    public class TransferRepository : ITransferRepository
//    {
//        private readonly IMapper _mapper;
//        private readonly AccountDbContext _accountDbContext;
//        private readonly IEmailRepository _emailRepository;
//        private readonly IOtpRepository _otpRepository;

//        public TransferRepository(IMapper mapper, AccountDbContext accountDbContext, IEmailRepository emailRepository, IOtpRepository otpRepository)
//        {
//            _mapper = mapper;
//            _accountDbContext = accountDbContext;
//            _emailRepository = emailRepository;
//            _otpRepository = otpRepository;
//        }

//        public async Task<TransferDto> CreateTransfer(TransferDto transferDto, string otpCode)
//        {
//            var isOtpVerified = await _otpRepository.VerifyOtp(transferDto.SenderId, otpCode);

//            if (!isOtpVerified)
//            {
//                throw new ArgumentException("OTP verification failed. Cannot proceed with transfer.");
//            }

//            if (transferDto.TransferAmount <= 0)
//            {
//                throw new ArgumentException("Transfer amount must be greater than zero");
//            }

//            using (var transaction = await _accountDbContext.Database.BeginTransactionAsync())
//            {
//                try
//                {
//                    var senderAccount = await _accountDbContext.Account
//                        .FirstOrDefaultAsync(e => e.AccountId == transferDto.SenderId);
//                    if (senderAccount == null)
//                    {
//                        throw new Exception("Sender account not found");
//                    }

//                    var receiverAccount = await _accountDbContext.Account
//                        .FirstOrDefaultAsync(e => e.AccountId == transferDto.ReceiverId);
//                    if (receiverAccount == null)
//                    {
//                        throw new Exception("Receiver account not found");
//                    }

//                    if (senderAccount.CurrentBalance < transferDto.TransferAmount)
//                    {
//                        throw new Exception("Insufficient funds");
//                    }

//                    senderAccount.CurrentBalance -= transferDto.TransferAmount;
//                    receiverAccount.CurrentBalance += transferDto.TransferAmount;

//                    _accountDbContext.Account.Update(senderAccount);
//                    _accountDbContext.Account.Update(receiverAccount);

//                    var transfer = _mapper.Map<Transfer>(transferDto);
//                    transfer.AccountId = senderAccount.Id;

//                    await _accountDbContext.Transfer.AddAsync(transfer);
//                    await _accountDbContext.SaveChangesAsync();
//                    await transaction.CommitAsync();

//                    var senderMailRequest = new MailRequest()
//                    {
//                        ToEmail = senderAccount.Email,
//                        Subject = "Transfer Confirmation",
//                        Body = $"{senderAccount.FirstName}, you have successfully transferred {transferDto.TransferAmount} to {receiverAccount.FirstName}. Your new balance is {senderAccount.CurrentBalance}."
//                    };
//                    await _emailRepository.SendEmailAsync(senderMailRequest);

//                    var receiverMailRequest = new MailRequest()
//                    {
//                        ToEmail = receiverAccount.Email,
//                        Subject = "Transfer Confirmation",
//                        Body = $"{receiverAccount.FirstName}, you have received {transferDto.TransferAmount} from {senderAccount.FirstName}. Your new balance is {receiverAccount.CurrentBalance}."
//                    };
//                    await _emailRepository.SendEmailAsync(receiverMailRequest);

//                    return _mapper.Map<TransferDto>(transfer);
//                }
//                catch (Exception ex)
//                {
//                    await transaction.RollbackAsync();
//                    throw new Exception("Error processing transfer", ex);
//                }
//            }
//        }
//    }
//}















