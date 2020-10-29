using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.Configuration
{
    public class ScheduleHourConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasOne(sh => sh.Patient)
                .WithOne(p => p.ScheduleHour)
                .HasForeignKey<Reservation>(sh => sh.PatientId);
        }
    }
}
