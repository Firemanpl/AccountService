using Microsoft.EntityFrameworkCore;

namespace AccountService.Entities
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserHistory> UserHistory { get; set; }


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
            modelBuilder.Entity<UserHistory>()
                .Property(ur => ur.VehicleId)
                .IsRequired();
            modelBuilder.Entity<UserHistory>()
                .Property(ur => ur.Paid)
                .IsRequired();
            modelBuilder.Entity<UserHistory>()
                .Property(ur => ur.KWh)
                .IsRequired();
            modelBuilder.Entity<UserHistory>()
                .Property(ur => ur.Kilometers)
                .IsRequired();
        }

    }
}