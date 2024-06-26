
using BankingSystem.Data;
using BankingSystem.Dto;
using BankingSystem.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    public class AccountController:ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        [HttpGet("GetAllAccount")]
        public async Task<object> Get()
        {
            try
            {
                var account = await _accountRepository.GetAllAccount();
                return Ok(account);
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpGet("GetById")]
        public async Task<object> GetAccountById(int Id)
        {
            try
            {
                var account = await _accountRepository.GetAccountById(Id);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("CreateAccount")]
        public async Task<ActionResult<AccountDto>>Create([FromBody] AccountDto accountDto)
        {
            try
            {
                var account = await _accountRepository.CreateAccount(accountDto);
                return Ok(account);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
