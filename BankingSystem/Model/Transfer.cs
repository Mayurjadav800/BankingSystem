using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.Model
{
    public class Transfer
    {
        [Key]
        public int Id { get; set; }

        // Foreign key
        [ForeignKey("Account")]
        public int AccountId { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public double TransferAmount { get; set; }

        // Navigation property
        public virtual Account Account { get; set; }
    }
}
