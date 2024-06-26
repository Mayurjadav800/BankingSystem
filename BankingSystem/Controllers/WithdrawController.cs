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

        public WithdrawController(IWithdrawRepository withdrawRepository)
        {
            _withdrawRepository = withdrawRepository;
        }
        [HttpPost("CreateWithdraw")]
        public async Task<ActionResult<WithdrawDto>>Create([FromBody] WithdrawDto withdrawDto)
        {
            try
            {
                var withdraw = await _withdrawRepository.CreateWithdraw(withdrawDto);
                return Ok(withdraw);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
