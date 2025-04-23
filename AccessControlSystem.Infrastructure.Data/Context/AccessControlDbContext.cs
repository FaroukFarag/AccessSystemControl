using AccessControlSystem.Domain.Models.AccessGroupDevices;
using AccessControlSystem.Domain.Models.AccessGroups;
using AccessControlSystem.Domain.Models.Devices;
using AccessControlSystem.Domain.Models.Roles;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.Domain.Models.Units;
using AccessControlSystem.Domain.Models.Users;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.AccessGroupDevices;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.AccessGroups;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Devices;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Roles;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Subscriptions;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Units;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccessControlSystem.Infrastructure.Data.Context;

public class AccessControlDbContext(DbContextOptions options) : IdentityDbContext<User, Role, int>(options)
{
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<AccessGroup> AccessGroups { get; set; }
    public DbSet<AccessGroupDevice> AccessGroupDevices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfigurations());
        modelBuilder.ApplyConfiguration(new RoleConfigurations());
        modelBuilder.ApplyConfiguration(new SubscriptionConfigurations());
        modelBuilder.ApplyConfiguration(new DeviceConfigurations());
        modelBuilder.ApplyConfiguration(new UnitConfigurations());
        modelBuilder.ApplyConfiguration(new AccessGroupConfigurations());
        modelBuilder.ApplyConfiguration(new AccessGroupDeviceConfigurations());
    }
}
