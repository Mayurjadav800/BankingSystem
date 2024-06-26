using BankingSystem.Dto;
using BankingSystem.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    public class DepositeController:ControllerBase
    {
        private readonly IDepositeRepository _depositeRepository;

        public DepositeController(IDepositeRepository depositeRepository)
        {
            _depositeRepository = depositeRepository;
        }
        [HttpPost("Deposite")]
        public async Task<ActionResult<DepositeDto>> CreateDeposite([FromBody]DepositeDto depositeDto)
        {
            try
            {
                var deposite = await _depositeRepository.CreateDepository(depositeDto);
                return Ok(deposite);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
        }
    }
}
