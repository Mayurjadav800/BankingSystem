using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;

namespace BankingSystem.Model
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
       
        [Required]
        public double CurrentBalance { get; set; }
        [Required]
        public int AccountNumber { get; set; }

        // Navigation properties
        public virtual ICollection<Deposite> Deposites { get; set; }
        public virtual ICollection<Transfer> Transfers { get; set; }
        public virtual ICollection<Withdraw> Withdraws { get; set; }
        public virtual ICollection<Otp> Otps { get; set; }
    }
}
