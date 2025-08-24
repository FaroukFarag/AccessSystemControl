using AccessControlSystem.Domain.Models.Cards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Cards;

public class CardConfigurations : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.HasOne(c => c.Unit)
            .WithMany(u => u.Cards)
            .HasForeignKey(c => c.UnitId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
