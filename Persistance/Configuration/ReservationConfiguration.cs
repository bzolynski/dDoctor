using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasOne(r => r.Patient)
                .WithOne(p => p.ScheduleHour)
                .HasForeignKey<Reservation>(r => r.PatientId);

            builder.HasIndex(r => r.PatientId).IsUnique(false);

            
                
        }
    }
}
