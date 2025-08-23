using AccessControlSystem.Domain.Models.Cards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Cards;

public class CardConfigurations : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.HasOne(c => c.Owner)
            .WithMany(o => o.Cards)
            .HasForeignKey(c => c.OwnerId);
    }
}
