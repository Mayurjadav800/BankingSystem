namespace BankingSystem.Dto
{
    public class DepositeDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public double DepositeAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
