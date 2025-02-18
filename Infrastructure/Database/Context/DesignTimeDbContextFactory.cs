using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Kaar.Infrastructure.Database.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<StockTrackingContext>
    {
        public StockTrackingContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Kaar"))
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<StockTrackingContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            builder.UseSqlServer(connectionString);

            return new StockTrackingContext(builder.Options);
        }
    }
} 