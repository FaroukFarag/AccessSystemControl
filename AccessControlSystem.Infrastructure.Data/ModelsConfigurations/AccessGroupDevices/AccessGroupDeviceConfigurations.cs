using AccessControlSystem.Domain.Models.AccessGroupDevices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControlSystem.Infrastructure.Data.ModelsConfigurations.AccessGroupDevices;

public class AccessGroupDeviceConfigurations : IEntityTypeConfiguration<AccessGroupDevice>
{
    public void Configure(EntityTypeBuilder<AccessGroupDevice> builder)
    {
        builder.HasKey(agd => new { agd.AccessGroupId, agd.DeviceId });

        builder.HasOne(agd => agd.AccessGroup)
            .WithMany(ag => ag.AccessGroupDevices)
            .HasForeignKey(agd => agd.AccessGroupId);

        builder.HasOne(agd => agd.Device)
            .WithMany(d => d.AccessGroupDevices)
            .HasForeignKey(agd => agd.DeviceId);
    }
}
