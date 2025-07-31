using AccessControlSystem.Domain.Models.AccessGroupUnits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControlSystem.Infrastructure.Data.ModelsConfigurations.AccessGroupUnits;

public class AccessGroupUnitConfigurations : IEntityTypeConfiguration<AccessGroupUnit>
{
    public void Configure(EntityTypeBuilder<AccessGroupUnit> builder)
    {
        builder.HasKey(agu => new { agu.AccessGroupId, agu.UnitId });

        builder.HasOne(agu => agu.AccessGroup)
            .WithMany(ag => ag.AccessGroupUnits)
            .HasForeignKey(agd => agd.AccessGroupId);

        builder.HasOne(agu => agu.Unit)
            .WithMany(u => u.AccessGroupUnits)
            .HasForeignKey(agu => agu.UnitId);
    }
}
