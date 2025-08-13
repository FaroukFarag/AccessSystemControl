using AccessControlSystem.Common.Interfaces.Subscriptions;
using AccessControlSystem.Domain.Interfaces.Services.Users;
using AccessControlSystem.Domain.Models.AccessGroupDevices;
using AccessControlSystem.Domain.Models.AccessGroups;
using AccessControlSystem.Domain.Models.AccessGroupUnits;
using AccessControlSystem.Domain.Models.Cards;
using AccessControlSystem.Domain.Models.Devices;
using AccessControlSystem.Domain.Models.Roles;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.Domain.Models.Units;
using AccessControlSystem.Domain.Models.Users;
using AccessControlSystem.Domain.Models.Visitors;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.AccessGroupDevices;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.AccessGroups;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.AccessGroupUnits;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Cards;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Devices;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Roles;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Subscriptions;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Units;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Users;
using AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Visitors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AccessControlSystem.Infrastructure.Data.Context;

public class AccessControlDbContext(
    DbContextOptions options,
    IUserContextService userContextService) : IdentityDbContext<User, Role, int>(options)
{
    private readonly IUserContextService _userContextService = userContextService;

    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<AccessGroup> AccessGroups { get; set; }
    public DbSet<AccessGroupDevice> AccessGroupDevices { get; set; }
    public DbSet<AccessGroupUnit> AccessGroupUnits { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Visitor> Visitors { get; set; }

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
        modelBuilder.ApplyConfiguration(new AccessGroupUnitConfigurations());
        modelBuilder.ApplyConfiguration(new CardConfigurations());
        modelBuilder.ApplyConfiguration(new VisitorConfigurations());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            if (typeof(ISubscriptionEntity).IsAssignableFrom(clrType))
            {
                var method = typeof(AccessControlDbContext)
                    .GetMethod(nameof(SetTenantFilter), BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.MakeGenericMethod(clrType);

                method?.Invoke(this, [modelBuilder]);
            }

            else if (clrType == typeof(Subscription))
            {
                var method = typeof(AccessControlDbContext)
                    .GetMethod(nameof(SetSubscriptionFilter), BindingFlags.NonPublic | BindingFlags.Instance);

                method?.Invoke(this, [modelBuilder]);
            }
        }
    }

    private void SetTenantFilter<TEntity>(ModelBuilder builder) where TEntity : class, ISubscriptionEntity
    {
        builder.Entity<TEntity>().HasQueryFilter(e =>
            _userContextService.IsAdmin() ||
            !_userContextService.IsAuthenticated() ||
            !_userContextService.HasSubscriptionId() ||
            e.SubscriptionId == _userContextService.GetSubscriptionId());
    }

    private void SetSubscriptionFilter(ModelBuilder builder)
    {
        builder.Entity<Subscription>().HasQueryFilter(s =>
            _userContextService.IsAdmin() ||
            !_userContextService.IsAuthenticated() ||
            !_userContextService.HasSubscriptionId() ||
            s.Id == _userContextService.GetSubscriptionId());
    }
}
