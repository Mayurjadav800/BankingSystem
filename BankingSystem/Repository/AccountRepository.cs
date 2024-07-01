using AutoMapper;
using BankingSystem.Data;
using BankingSystem.Dto;
using BankingSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMapper _mapper;
        private readonly AccountDbContext _accountDbContext;
        public AccountRepository(IMapper mapper,AccountDbContext accountDbContext)
        {
            _mapper = mapper;
            _accountDbContext = accountDbContext;
        }
        public async Task<AccountDto> CreateAccount(AccountDto accountDto)
        {
            var existingAccount = await _accountDbContext.Account
        .FirstOrDefaultAsync(a => a.AccountNumber == accountDto.AccountNumber);
            if (existingAccount != null)
            {
                throw new Exception("Account number already exists");
            }
            if (accountDto.CurrentBalance < 0)
            {
                throw new Exception("Current balance cannot be negative");
            }
            var account = _mapper.Map<Account>(accountDto);
            account.CurrentBalance = Math.Round(accountDto.CurrentBalance, 2);
            //account.Password = accountDto.Password;
            await _accountDbContext.AddAsync(account);
            await _accountDbContext.SaveChangesAsync();
            return _mapper.Map<AccountDto>(account);
        }
        public async Task<AccountDto> GetAccountById(int Id)
        {
            var account = await _accountDbContext.Account.FindAsync(Id);
            if(account == null)
            {
                return null;
            }
            return _mapper.Map<AccountDto>(account);
        }
        public async Task<List<AccountDto>> GetAllAccount()
        {
            var account = await _accountDbContext.Account.ToListAsync();
            return _mapper.Map<List<AccountDto>>(account);
            
        }
    }
}
