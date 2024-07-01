﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.Model
{
    public class Otp
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        [Required]
        [Range(100000, 999999, ErrorMessage = "OTP Code must be a 6-digit number")]
        public string Code { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }

        public virtual Account Account { get; set; }

       
    }
}
