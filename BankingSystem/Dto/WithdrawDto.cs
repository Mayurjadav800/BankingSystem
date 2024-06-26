namespace BankingSystem.Dto
{
    public class WithdrawDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public double WithdrawAmount { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
