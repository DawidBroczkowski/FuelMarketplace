using FuelMarketplace.Application.Services;
using FuelMarketplace.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FuelMarketplace.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IValidationService, ValidationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IModerationService, ModerationService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IOfferService, OfferService>();
            services.AddTransient<ISalesPointService, SalesPointService>();

            return services;
        }
    }
}
