namespace BankingSystem.Dto
{
    public class WithdrawDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public double WithdrawAmount { get; set; }
    //    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    }
}
