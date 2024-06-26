using AutoMapper;
using BankingSystem.Data;
using BankingSystem.Dto;
using BankingSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Repository
{
    public class WithdraRepository : IWithdrawRepository
    {
        private readonly IMapper _mapper;
        private readonly AccountDbContext _accountDbContext;

        public WithdraRepository(IMapper mapper, AccountDbContext accountDbContext)
        {
            _mapper = mapper;
            _accountDbContext = accountDbContext;
        }
        public async Task<WithdrawDto> CreateWithdraw(WithdrawDto withdrawDto)
        {
            if (withdrawDto.WithdrawAmount <= 0)
            {
                throw new ArgumentException("Deposite amount must be greater than zero");
            }
            using (var transcation = await _accountDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var account = await _accountDbContext.Account
                        .FirstOrDefaultAsync(e => e.AccountId == withdrawDto.AccountId);

                    if (account == null)
                    {
                        throw new Exception("Account not found");
                    }
                    var withdraw = _mapper.Map<Withdraw>(withdrawDto);
                    withdraw.AccountId = account.Id;
                    await _accountDbContext.Withdraw.AddAsync(withdraw);
                    await _accountDbContext.SaveChangesAsync();
                    var withdraws = _mapper.Map<WithdrawDto>(withdraw);

                    account.CurrentBalance = account.CurrentBalance - withdrawDto.WithdrawAmount;
                    _accountDbContext.Account.Update(account);
                    await _accountDbContext.SaveChangesAsync();
                    transcation.CommitAsync();
                    return _mapper.Map<WithdrawDto>(withdrawDto);
                }
                catch (Exception ex)
                {
                    await transcation.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
