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

        builder.Property(u => u.Area)
            .HasPrecision(18, 6)
            .IsRequired();

        builder.HasMany(u => u.Owners)
            .WithOne(o => o.Unit)
            .HasForeignKey(o => o.UnitId)
            .IsRequired(false);

        builder.HasMany(u => u.AccessGroupUnits)
            .WithOne(agu => agu.Unit)
            .HasForeignKey(agu => agu.UnitId);
    }
}
