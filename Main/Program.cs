using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Api.Controllers;
using Api.Profiles;
using Application.Services;
using Infrastructure.Mappings;
using Infrastructure.Services;
using Kaar.Infrastructure.Database.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Kaar.BackgroundServices.Services;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.ConfigureServices((context, services) =>
        {
            //App Part Loading as controller is in different assembly
            services.AddControllers()
                .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(StockTrackingController).Assembly));
            
            // Add Swagger services
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Configure DbContext
            services.AddDbContext<StockTrackingContext>(options =>
                options.UseSqlServer(
                    context.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Infrastructure")));

            // Register AutoMapper
            services.AddAutoMapper(cfg => {
                cfg.AddMaps(new[] {
                    typeof(DomainMappingProfile).Assembly,  // Infrastructure assembly
                    typeof(StockTrackingController).Assembly // API assembly
                });
            });

            // Register services
            services.AddScoped<IStockPriceService, StockPriceService>();
            services.AddScoped<IUserPreferenceService, UserPreferenceService>();
            services.AddHostedService<StockPriceMonitorService>();
            services.AddScoped<INotificationService, ConsoleNotificationService>();
        });

        webBuilder.Configure((context, app) =>
        {
            if (context.HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Configure Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockX API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        });
    });

var app = builder.Build();
app.Run();
