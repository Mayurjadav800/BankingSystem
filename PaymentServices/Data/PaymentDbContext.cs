using BankingSystem.Data;
using Microsoft.EntityFrameworkCore;
using PaymentMicroServices.Model;

namespace PaymentMicroServices.Data
{
    public class PaymentDbContext:DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext>options):base(options)
        {
            
        }
        public DbSet<Compund> Compund { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Compund>().HasKey(e => e.Id);
        }
        
    }
}
