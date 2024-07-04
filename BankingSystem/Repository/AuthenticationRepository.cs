using AutoMapper;
using BankingSystem.Data;
using BankingSystem.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankingSystem.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IMapper _mapper;
        private readonly AccountDbContext _accountDbContext;
        private readonly IConfiguration _configuration;

        public AuthenticationRepository(IMapper mapper, AccountDbContext accountDbContext,IConfiguration configuration)
        {
            _mapper = mapper;
            _accountDbContext = accountDbContext;
            _configuration = configuration;
        }
        public async Task<string> CreateAuthentication(LogginDto logginDto)
        {
            try
            {
                var user = _accountDbContext.Account.Where(u => u.Email == logginDto.Email && u.Password == logginDto.Password).Count();

                if (user == 0)
                {
                    throw new UnauthorizedAccessException("Invalid credentials");
                }
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier,logginDto.Email),

            };
                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
