using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.Model
{
    public class Deposite
    {
        [Key]
        public int Id { get; set; }

        //// Foreign key
        [ForeignKey("Account")]
        public int AccountId { get; set; }

        [Required]
        public double DepositeAmount { get; set; }
        [Required]

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        // Navigation 
        public virtual Account Account { get; set; }
    }
}
