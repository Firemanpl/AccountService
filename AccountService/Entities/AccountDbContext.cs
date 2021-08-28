using AccountService.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace AccountService.Entities
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserPayments> UserPayments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.PhoneNumber)
                .IsRequired()
                .HasMaxLength(9);
            modelBuilder.Entity<User>()
                .Property(u => u.Nationality)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.VeryficationCode)
                .IsRequired()
                .HasMaxLength(8);
            modelBuilder.Entity<UserPayments>()
                .Property(ur => ur.VehicleId)
                .IsRequired();
            modelBuilder.Entity<UserPayments>()
                .Property(ur => ur.Payment)
                .IsRequired();
            modelBuilder.Entity<UserPayments>()
                .Property(cu => cu.Currency)
                .IsRequired()
                .HasMaxLength(3);
            modelBuilder.Entity<UserPayments>()
                .Property(ur => ur.KWh)
                .IsRequired();
            modelBuilder.Entity<UserPayments>()
                .Property(ur => ur.Kilometers)
                .IsRequired();
            modelBuilder.Entity<Role>()
                .Property(n=>n.Name)
                .IsRequired();
        }
    }
}