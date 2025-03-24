using AccessControlSystem.Domain.Models.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Subscriptions;

public class SubscriptionConfigurations : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.Property(l => l.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}
