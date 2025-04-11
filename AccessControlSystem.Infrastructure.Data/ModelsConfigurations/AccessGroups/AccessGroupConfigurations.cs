using AccessControlSystem.Domain.Models.AccessGroups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControlSystem.Infrastructure.Data.ModelsConfigurations.AccessGroups;

public class AccessGroupConfigurations : IEntityTypeConfiguration<AccessGroup>
{
    public void Configure(EntityTypeBuilder<AccessGroup> builder)
    {
        builder.Property(ag => ag.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}
