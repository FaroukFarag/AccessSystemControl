using AccessControlSystem.Domain.Models.Visitors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Visitors;

public class VisitorConfigurations : IEntityTypeConfiguration<Visitor>
{
    public void Configure(EntityTypeBuilder<Visitor> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(v => v.Unit)
            .WithMany(u => u.Visitors)
            .HasForeignKey(v => v.UnitId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
