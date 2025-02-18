using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Kaar.Domain.Entities;

namespace Kaar.Infrastructure.Database.Configurations
{
    public class StockPriceConfiguration : IEntityTypeConfiguration<StockPrice>
    {
        public void Configure(EntityTypeBuilder<StockPrice> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.StockSymbol)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(e => e.Price)
                .HasPrecision(18, 4);  // 18 total digits, 4 decimal places

            builder.HasIndex(e => e.StockSymbol);
        }
    }
} 