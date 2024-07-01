using BankingSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Data
{
    public class AccountDbContext:DbContext
    {
        public AccountDbContext(DbContextOptions<AccountDbContext>options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Account and Deposite relationship
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Deposites)
                .WithOne(d => d.Account)
                .HasForeignKey(d => d.AccountId);

            // Account and Transfer relationship
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Transfers)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId);

            // Account and Withdraw relationship
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Withdraws)
                .WithOne(w => w.Account)
                .HasForeignKey(w => w.AccountId);

            modelBuilder.Entity<Otp>()
               .HasOne(o => o.Account)
               .WithMany(a => a.Otps)
               .HasForeignKey(o => o.AccountId);

        }
        public DbSet<Account> Account { get; set; }
        public DbSet<Deposite> Deposite { get; set; }
        public DbSet<Transfer>Transfer { get; set; }
        public DbSet<Withdraw>Withdraw { get; set; }
        public DbSet<Otp> Otp { get; set; }

    }

}
