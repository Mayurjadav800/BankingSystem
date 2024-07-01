using BankingSystem.Dto;
using BankingSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    public class OtpController:ControllerBase
    {
        private readonly IOtpRepository _otpRepository;
        private readonly ILogger<OtpController> _logger;

        public OtpController(IOtpRepository otpRepository,ILogger<OtpController>logger)
        {
            _otpRepository = otpRepository;
            _logger = logger;
        }
        [HttpPost("Otp-Generate")]
        //[Authorize]
        public async Task<IActionResult> GenerateOtp(int accountId)
        {
            try
            {
                _logger.LogInformation("Create API for generate the otp");
                var otpCode = await _otpRepository.GenerateOtp(accountId);
                return Ok(new { OtpCode = otpCode });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("faild to generate the otp");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("verify")]
        //[Authorize]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpDto otpDto)
        {
            try
            {
                _logger.LogInformation("Create the api for verify the otp");
                var isVerified = await _otpRepository.VerifyOtp(otpDto.AccountId, otpDto.Code);
                return Ok(isVerified);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to verify OTP");
                return StatusCode(500, "Internal server error");
            }
        }



    }
}
