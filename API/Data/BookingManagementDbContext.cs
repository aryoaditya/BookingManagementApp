using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class BookingManagementDbContext : DbContext
    {
        public BookingManagementDbContext(DbContextOptions<BookingManagementDbContext> options) : base(options) { }

        // Add Models to migrate
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<University> Universities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.Nik).IsUnique(); // Menambahkan constraint UNIQUE untuk NIK
                entity.HasIndex(e => e.Email).IsUnique(); // Menambahkan constraint UNIQUE untuk Email
                entity.HasIndex(e => e.PhoneNumber).IsUnique(); // Menambahkan constraint UNIQUE untuk PhoneNumber
            });

            // One University has many Educations
            modelBuilder.Entity<University>()
            .HasMany(e => e.Educations)
            .WithOne(u => u.University)
            .HasForeignKey(e => e.UniversityGuid)
            .OnDelete(DeleteBehavior.Restrict);

            // One Education has one Employee
            modelBuilder.Entity<Education>()
            .HasOne(e => e.Employee)
            .WithOne(e => e.Education)
            .HasForeignKey<Education>(e => e.Guid);

            // One Room has many Bookings
            modelBuilder.Entity<Room>()
                .HasMany(b => b.Bookings)
                .WithOne(r => r.Room)
                .HasForeignKey(b => b.RoomGuid);

            // One Employee has many Bookings
            modelBuilder.Entity<Employee>()
                .HasMany(b => b.Bookings)
                .WithOne(e => e.Employee)
                .HasForeignKey(b => b.EmployeeGuid);

            // One Employee has one Account
            modelBuilder.Entity<Employee>()
                .HasOne(a => a.Account)
                .WithOne(e => e.Employee)
                .HasForeignKey<Account>(a => a.Guid);

            // One Account has many Account Roles
            modelBuilder.Entity<Account>()
                .HasMany(a => a.AccountRoles)
                .WithOne(a => a.Account)
                .HasForeignKey(a => a.AccountGuid);

            // One Role has many Account Roles
            modelBuilder.Entity<Role>()
                .HasMany(a => a.AccountRoles)
                .WithOne(r => r.Role)
                .HasForeignKey(a => a.RoleGuid);

        }

    }
}
