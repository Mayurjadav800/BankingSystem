﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PaymentMicroServices.Dto;
using PaymentMicroServices.Repository;

namespace PaymentMicroServices.Controllers
{
    public class CompundController:ControllerBase
    {
        private readonly ICompundRepository _compundRepository;

        public CompundController(ICompundRepository compundRepository)
        {
            _compundRepository = compundRepository;
        }

        [HttpGet("GetallDetails")]
        public async Task<object> Get()
        {
            try
            {
                var compund = await _compundRepository.GetAll();
                return Ok(compund);
                

            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
            [HttpPost("CreateCompund")]
        public async Task<ActionResult<CompundDto>>Create([FromBody] CompundDto compundDto)
        {
            try
            {
                var amount = await _compundRepository.CreateCompund(compundDto);
                return Ok(amount);

            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
