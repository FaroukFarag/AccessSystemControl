using AccessControlSystem.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Users;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.SubscriptionId)
            .IsRequired(false)
            .HasDefaultValue(null);

        builder.HasOne(u => u.Subscription)
            .WithMany(s => s.Users)
            .HasForeignKey(u => u.SubscriptionId);
    }
}
