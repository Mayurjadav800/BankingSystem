using BankingSystem.Dto;
using BankingSystem.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    public class TransferController:ControllerBase
    {
        private readonly ITransferRepository _transferRepository;

        public TransferController(ITransferRepository transferRepository)
        {
            _transferRepository = transferRepository;
        }
        [HttpPost("CreateForTransfer")]
        public async Task<ActionResult<TransferDto>>Create([FromBody]TransferDto transferDto)
        {
            try
            {
                var transfer = await _transferRepository.CreateTransfer(transferDto);
                return Ok(transfer);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
        }
    }
}
