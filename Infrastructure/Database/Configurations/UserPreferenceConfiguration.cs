using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Kaar.Domain.Entities;

namespace Kaar.Infrastructure.Database.Configurations
{
    public class UserPreferenceConfiguration : IEntityTypeConfiguration<UserPreference>
    {
        public void Configure(EntityTypeBuilder<UserPreference> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.StockSymbol)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(e => e.ThresholdPrice)
                .HasPrecision(18, 4);  // 18 total digits, 4 decimal places

            builder.HasIndex(e => new { e.UserId, e.StockSymbol }).IsUnique();
        }
    }
} 