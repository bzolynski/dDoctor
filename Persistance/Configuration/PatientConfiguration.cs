using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasOne(p => p.Address)
                .WithOne(a => a.Patient)
                .HasForeignKey<Patient>(p => p.AddressId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
