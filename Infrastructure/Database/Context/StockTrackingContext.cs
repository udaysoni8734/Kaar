using Microsoft.EntityFrameworkCore;
using Kaar.Domain.Entities;

namespace Kaar.Infrastructure.Database.Context
{
    public class StockTrackingContext : DbContext
    {
        public StockTrackingContext(DbContextOptions<StockTrackingContext> options)
            : base(options)
        {
        }

        public DbSet<StockPrice> StockPrices { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StockTrackingContext).Assembly);
        }
    }
} 