using AccessControlSystem.Domain.Models.SubscriptionsDevices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControlSystem.Infrastructure.Data.ModelsConfigurations.SubscriptionsDevices;

public class SubscriptionDeviceConfigurations : IEntityTypeConfiguration<SubscriptionDevice>
{
    public void Configure(EntityTypeBuilder<SubscriptionDevice> builder)
    {
        builder.HasKey(sd => new { sd.SubscriptionId, sd.DeviceId });

        builder.HasOne(sd => sd.Subscription)
            .WithMany(s => s.SubscriptionsDevices)
            .HasForeignKey(sd => sd.SubscriptionId);

        builder.HasOne(sd => sd.Device)
            .WithMany(d => d.SubscriptionsDevices)
            .HasForeignKey(sd => sd.DeviceId);
    }
}
