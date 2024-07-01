using BankingSystem.Dto;
using BankingSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    public class TransferController:ControllerBase
    {
        private readonly ITransferRepository _transferRepository;
        private readonly ILogger<TransferController> _logger;
        private readonly IOtpRepository _otpRepository;

        public TransferController(ITransferRepository transferRepository,ILogger<TransferController>logger,IOtpRepository otpRepository)
        {
            _transferRepository = transferRepository;
            _logger = logger;
            _otpRepository = otpRepository;
        }
        [HttpPost("CreateForTransfer")]
        //[Authorize]
        public async Task<ActionResult<TransferDto>> Create([FromBody] TransferDto transferDto)
        {
            try
            {
                _logger.LogInformation("Create API for Transfer the Amount One to Another.");
                var transfer = await _transferRepository.CreateTransfer(transferDto);
                return Ok(transfer);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Faild to transfer amount One to Another");
                return StatusCode(500, ex.Message);
            }
        }




        //[HttpPost("create")]
        //public async Task<IActionResult> CreateTransfer([FromBody] TransferDto transferDto, string otpCode)
        //{
        //    try
        //    {
        //        var transfer = await _transferRepository.CreateTransfer(transferDto, otpCode);
        //        return Ok(transfer);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new { Message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Message = ex.Message });
        //    }
        //}
    }
}
