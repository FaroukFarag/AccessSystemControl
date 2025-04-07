using AccessControlSystem.Domain.Models.Roles;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.Domain.Models.Users;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Devices;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Roles;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Subscriptions;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccessControlSystem.Infrastructure.Data.Context;

public class AccessControlDbContext(DbContextOptions options) : IdentityDbContext<User, Role, int>(options)
{
    public DbSet<Subscription> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfigurations());
        modelBuilder.ApplyConfiguration(new RoleConfigurations());
        modelBuilder.ApplyConfiguration(new SubscriptionConfigurations());
        modelBuilder.ApplyConfiguration(new DeviceConfigurations());
    }
}
