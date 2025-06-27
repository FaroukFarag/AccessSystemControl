using AccessControlSystem.Domain.Models.UnitAccessGroups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControlSystem.Infrastructure.Data.ModelsConfigurations.UnitAccessGroups;

public class UnitAccessGroupConfigurations : IEntityTypeConfiguration<UnitAccessGroup>
{
    public void Configure(EntityTypeBuilder<UnitAccessGroup> builder)
    {
        builder.HasKey(uag => new { uag.AccessGroupId, uag.UnitId });

        builder.HasOne(uag => uag.AccessGroup)
            .WithMany(ag => ag.UnitAccessGroups)
            .HasForeignKey(uag => uag.AccessGroupId);

        builder.HasOne(uag => uag.Unit)
            .WithMany(u => u.UnitAccessGroups)
            .HasForeignKey(uag => uag.UnitId);
    }
}
