using AccessControlSystem.Domain.Models.AccessGroups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControlSystem.Infrastructure.Data.ModelsConfigurations.AccessGroups;

public class AccessGroupConfigurations : IEntityTypeConfiguration<AccessGroup>
{
    public void Configure(EntityTypeBuilder<AccessGroup> builder)
    {
        builder.Property(ag => ag.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(ag => ag.AccessGroupUnits)
            .WithOne(agu => agu.AccessGroup)
            .HasForeignKey(agu => agu.UnitId);

        builder.HasMany(ag => ag.AccessGroupDevices)
            .WithOne(agd => agd.AccessGroup)
            .HasForeignKey(agd => agd.AccessGroupId);
    }
}
