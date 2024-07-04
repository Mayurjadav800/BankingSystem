using AutoMapper;
using BankingSystem.Data;
using BankingSystem.Dto;
using BankingSystem.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

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
        public async Task<object> CreateTransfer(TransferDto transferDto)
        {
            try
            {
                var otp = await _accountDbContext.Otp
                    .FirstOrDefaultAsync(e => e.AccountId == transferDto.AccountId && e.IsUsed == true);

                if (otp == null)
                {
                    return "invalid otp.";
                }
                var dateTime = DateTime.Now;
                if (dateTime > otp.ExpiryDate)
                {
                    _accountDbContext.Remove(otp);
                    _accountDbContext.SaveChanges();
                    return "OTP will be expire";
                }

                if (transferDto.TransferAmount <= 0)
                {
                    //throw new ArgumentException("Transfer amount must be greater than zero.");
                    return "Transfer amount must be greater than zero";
                }
                using (var transaction = await _accountDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Retrieve sender account
                        var senderAccount = await _accountDbContext.Account
                            .FirstOrDefaultAsync(e => e.Id == transferDto.SenderId);
                        if (senderAccount == null)
                        {
                            throw new Exception("Sender account not found.");
                        }
                        var receiverAccount = await _accountDbContext.Account
                            .FirstOrDefaultAsync(e => e.Id == transferDto.ReceiverId);
                        if (receiverAccount == null)
                        {
                            //throw new Exception("Receiver account not found.");
                            return "Receiver amount not found";
                        }
                        // Check  balance
                        if (senderAccount.CurrentBalance < transferDto.TransferAmount)
                        {
                            return "Insufficient funds";
                        }
                        senderAccount.CurrentBalance -= transferDto.TransferAmount;
                        receiverAccount.CurrentBalance += transferDto.TransferAmount;
                        _accountDbContext.Account.Update(senderAccount);
                        _accountDbContext.Account.Update(receiverAccount);
                        var transfer = _mapper.Map<Transfer>(transferDto);
                        await _accountDbContext.Transfer.AddAsync(transfer);
                        //otp.IsUsed = true;
                        //_accountDbContext.Otp.Update(otp);
                        await _accountDbContext.SaveChangesAsync();
                        _accountDbContext.Otp.Remove(otp);
                        _accountDbContext.SaveChanges();
                        await transaction.CommitAsync();
                        var senderEmailRequest = new MailRequest
                        {
                            ToEmail = senderAccount.Email,
                            Subject = "Transfer Confirmation",
                            Body = $"{senderAccount.FirstName}, you have successfully transferred {transferDto.TransferAmount} to {receiverAccount.FirstName}. Your new balance is {senderAccount.CurrentBalance}."
                        };
                        await _emailRepository.SendEmailAsync(senderEmailRequest);

                        var receiverEmailRequest = new MailRequest
                        {
                            ToEmail = receiverAccount.Email,
                            Subject = "Transfer Confirmation",
                            Body = $"{receiverAccount.FirstName}, you have received {transferDto.TransferAmount} from {senderAccount.FirstName}. Your new balance is {receiverAccount.CurrentBalance}."
                        };
                        await _emailRepository.SendEmailAsync(receiverEmailRequest);
                       // await GenerateNewOtp(transferDto.AccountId);

                        var transferResult = _mapper.Map<TransferDto>(transfer);
                        return transferResult;
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating transfer", ex);
            }
        }
    }
}

































