using AccessControlSystem.Domain.Lookups.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControlSystem.Infrastructure.Data.ModelsConfigurations.Lookups.Abstraction;

public class BaseLookupConfiguration<TEntity, TEnum>(string tableName) : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseLookup, new()
    where TEnum : Enum
{
    private readonly string _tableName = tableName;

    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.ToTable(_tableName);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
           .ValueGeneratedNever();

        builder.Property(e => e.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasData(
            Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new TEntity { Id = Convert.ToInt32(e), Name = e.ToString() })
        );
    }
}
