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
            modelBuilder.Entity<Account>()
               .HasMany(a => a.Deposites)
               .WithOne(d => d.Account)
               .HasForeignKey(d => d.AccountId);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Transfers)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Withdraws)
                .WithOne(w => w.Account)
                .HasForeignKey(w => w.AccountId);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Otps)
                .WithOne(o => o.Account)
                .HasForeignKey(o => o.AccountId);

            base.OnModelCreating(modelBuilder);

        }
        public DbSet<Account> Account { get; set; }
        public DbSet<Deposite> Deposite { get; set; }
        public DbSet<Transfer>Transfer { get; set; }
        public DbSet<Withdraw>Withdraw { get; set; }
        public DbSet<Otp> Otp { get; set; }

    }

}
