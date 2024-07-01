using BankingSystem.Dto;
using BankingSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    public class DepositeController:ControllerBase
    {
        private readonly IDepositeRepository _depositeRepository;
        private readonly ILogger<DepositeController> _logger;

        public DepositeController(IDepositeRepository depositeRepository,ILogger<DepositeController>logger)
        {
            _depositeRepository = depositeRepository;
            _logger = logger;
        }
        [HttpPost("Deposite")]
        [Authorize]
        public async Task<ActionResult<DepositeDto>> CreateDeposite([FromBody]DepositeDto depositeDto)
        {
            try
            {
                _logger.LogInformation("Create API for the Deposite amount");
                var deposite = await _depositeRepository.CreateDepository(depositeDto);
                return Ok(deposite);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed to Deposite Process");
                return StatusCode(500, ex.Message);

            }
        }
    }
}
