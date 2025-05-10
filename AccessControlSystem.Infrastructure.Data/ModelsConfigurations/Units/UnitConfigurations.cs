using AccessControlSystem.Domain.Models.Units;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Units;

public class UnitConfigurations : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Area)
            .HasPrecision(18, 6)
            .IsRequired();
    }
}
