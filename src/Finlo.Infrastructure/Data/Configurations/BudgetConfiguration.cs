using Finlo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finlo.Infrastructure.Data.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Budget> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Limit)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(b => b.Month)
            .IsRequired();
        
        builder.Property(b => b.Year)
            .IsRequired();
    }
}