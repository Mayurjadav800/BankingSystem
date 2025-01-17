﻿using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Dto
{
    public class AccountDto
    {
        public int Id { get; set; }
        //public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double CurrentBalance { get; set; }
        public int AccountNumber { get; set; }
    }
}
