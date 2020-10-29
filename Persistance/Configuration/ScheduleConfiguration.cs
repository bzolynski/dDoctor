using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.Configuration
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            
            
            builder.HasOne(s => s.Doctor)
                .WithMany(d => d.Schedules)
                .HasForeignKey(s => s.DoctorId);

            builder.HasOne(s => s.Specialization)
                .WithMany(sp => sp.Schedules)
                .HasForeignKey(s => s.SpecializationId);

            builder.HasMany(s => s.Reservations)
                .WithOne(sh => sh.Schedule)
                .HasForeignKey(sh => sh.ScheduleId);
        }
    }
}
