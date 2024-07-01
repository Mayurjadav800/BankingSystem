﻿namespace BankingSystem.Dto
{
    public class TransferDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int SenderId { get; set; }
        
        public int ReceiverId { get; set; }
        //public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public double TransferAmount { get; set; }
    }
}
