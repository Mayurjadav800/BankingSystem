using BankingSystem.Dto;
using BankingSystem.Model;
using BankingSystem.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    public class WithdrawController:ControllerBase
    {
        private readonly IWithdrawRepository _withdrawRepository;
        private readonly ILogger<WithdrawController> _logger;

        public WithdrawController(IWithdrawRepository withdrawRepository,ILogger<WithdrawController>logger)
        {
            _withdrawRepository = withdrawRepository;
            _logger = logger;
        }
        [HttpPost("CreateWithdraw")]
        public async Task<ActionResult<WithdrawDto>>Create([FromBody] WithdrawDto withdrawDto)
        {
            try
            {
                _logger.LogInformation("Create API for the Withdraw Amount");
                var withdraw = await _withdrawRepository.CreateWithdraw(withdrawDto);
                return Ok(withdraw);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Faild to Withdraw the Amount");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
