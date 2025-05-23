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

        builder.HasMany(u => u.UserRoles)
              .WithOne()
              .HasForeignKey(ur => ur.UserId);

        builder.HasMany(o => o.Units)
            .WithOne(u => u.Owner)
            .HasForeignKey(u => u.OwnerId);

        builder.HasMany(u => u.AccessGroups)
            .WithOne(ag => ag.Owner)
            .HasForeignKey(u => u.OwnerId);
    }
}
