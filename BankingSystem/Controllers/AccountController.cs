
using BankingSystem.Data;
using BankingSystem.Dto;
using BankingSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    public class AccountController:ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountController> _logger;
        private readonly IAuthenticationRepository _authenticationRepository;

        public AccountController(IAccountRepository accountRepository,ILogger<AccountController>logger,IAuthenticationRepository authenticationRepository)
        {
            _accountRepository = accountRepository;
            _logger = logger;
            _authenticationRepository = authenticationRepository;
        }
        [HttpGet("GetAllAccount")]
      // [Authorize]
        public async Task<object> Get()
        {
            try
            {
                _logger.LogInformation("Create API for listing of all Accounts");
                var account = await _accountRepository.GetAllAccount();
                return Ok(account);
            }catch(Exception ex)
            {
                _logger.LogInformation("Failed to listings all account");
                return StatusCode(500,ex.Message);
            }
        }
        [HttpGet("GetById")]
        [Authorize]
        public async Task<object> GetAccountById(int Id)
        {
            try
            {
                _logger.LogInformation("Listing the User Account By Id");
                var account = await _accountRepository.GetAccountById(Id);
                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed to Listing All Accounts");
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("Login")]
        //[AllowAnonymous]
        
        public async Task<ActionResult<string>> Login([FromBody] LogginDto logginDto)
        {
            try
            {
                var token = await _authenticationRepository.CreateAuthentication(logginDto);
                return Ok(token);
            }
            //catch (UnauthorizedAccessException ex)
            //{
            //    return Unauthorized(ex.Message);
            //}
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("CreateAccount")]
        [Authorize]
        public async Task<ActionResult<AccountDto>>Create([FromBody] AccountDto accountDto)
        {
            try
            {
                _logger.LogInformation("Create API for Create Account");
                var account = await _accountRepository.CreateAccount(accountDto);
                return Ok(account);

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed to Create the Account");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
