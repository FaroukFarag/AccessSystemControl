using AccessControlSystem.Domain.Models.Devices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Devices;

public class DeviceConfigurations : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.Property(d => d.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.MacAddress)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(d => d.Subscription)
            .WithMany(s => s.Devices)
            .HasForeignKey(d => d.SubscriptionId);
    }
}
