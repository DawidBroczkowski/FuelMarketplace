using FuelMarketplace.Infrastructure.DataAccess;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FuelMarketplace.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add entity framework database context with transient lifetime
            services.AddDbContext<MarketplaceContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default"),
                b => b.MigrationsAssembly(typeof(MarketplaceContext).Assembly.FullName)), ServiceLifetime.Transient);

            // Set up dependency injection
            services.AddTransient<IUserRepository, DbUserRepository>();
            services.AddTransient<IPostRepository, DbPostRepository>();
            services.AddTransient<IOfferRepository, DbOfferRepository>();
            services.AddTransient<ISalesPointRepository, DbSalesPointRepository>();
            services.AddTransient<ICommentRepository, DbCommentRepository>();
            services.AddTransient<IImageRepository, InStorageImageRepository>();
            
            return services;
        }

        public static IServiceProvider EnsureDatabaseIsCreated(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MarketplaceContext>();
                dbContext.Database.EnsureCreated();
            }

            return services;
        }
    }
}
