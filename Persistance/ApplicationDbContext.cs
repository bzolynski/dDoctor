using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistance
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Registrant> Registrants{ get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Specialization> Specializations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Account)
                .WithOne(a => a.Doctor)
                .HasForeignKey<Doctor>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Registrant>()
                .HasOne(r => r.Account)
                .WithOne(a => a.Registrant)
                .HasForeignKey<Registrant>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
