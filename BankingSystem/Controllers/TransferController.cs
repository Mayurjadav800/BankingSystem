using BankingSystem.Dto;
using BankingSystem.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    public class TransferController:ControllerBase
    {
        private readonly ITransferRepository _transferRepository;
        private readonly ILogger<TransferController> _logger;

        public TransferController(ITransferRepository transferRepository,ILogger<TransferController>logger)
        {
            _transferRepository = transferRepository;
            _logger = logger;
        }
        [HttpPost("CreateForTransfer")]
        public async Task<ActionResult<TransferDto>>Create([FromBody]TransferDto transferDto)
        {
            try
            {
                _logger.LogInformation("Create API for Transfer the Amount One to Another.");
                var transfer = await _transferRepository.CreateTransfer(transferDto);
                return Ok(transfer);
            }
            catch(Exception ex)
            {
                _logger.LogInformation("Faild to transfer amount One to Another");
                return StatusCode(500, ex.Message); 
            }
        }
    }
}
