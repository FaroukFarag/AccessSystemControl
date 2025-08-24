using AccessControlSystem.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Users;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.SubscriptionId);

        builder.HasOne(u => u.Subscription)
            .WithMany(s => s.Users)
            .HasForeignKey(u => u.SubscriptionId);

        builder.HasMany(u => u.UserRoles)
              .WithOne()
              .HasForeignKey(ur => ur.UserId);

        builder.HasOne(o => o.Unit)
            .WithMany(u => u.Owners)
            .HasForeignKey(o => o.UnitId)
            .IsRequired(false);
    }
}
