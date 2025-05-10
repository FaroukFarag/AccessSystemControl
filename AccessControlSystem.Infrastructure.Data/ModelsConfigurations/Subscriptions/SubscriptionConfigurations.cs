using AccessControlSystem.Domain.Models.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Subscriptions;

public class SubscriptionConfigurations : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.Property(s => s.CustomerName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.PaymentPerMonth)
            .HasPrecision(18, 6)
            .IsRequired();

        builder.HasMany(s => s.Users)
            .WithOne(u => u.Subscription)
            .HasForeignKey(u => u.SubscriptionId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
