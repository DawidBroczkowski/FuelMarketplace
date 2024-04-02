using FuelMarketplace.Domain.Models;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using System.Security.Claims;

namespace FuelMarketplace.API
{
    public class AuthorizationManager
    {
        private IUserRepository _userRepository;

        public AuthorizationManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public WebApplicationBuilder ConfigureAuthorization(WebApplicationBuilder builder)
        {

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        string email = context.User.FindFirstValue(ClaimTypes.Email)!;
                        var role = _userRepository.GetUserRoleAsync(email).Result;
                        return role is Role.administrator;
                    });
                });

                options.AddPolicy("AdminOrMod", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        string email = context.User.FindFirstValue(ClaimTypes.Email)!;
                        var role = _userRepository.GetUserRoleAsync(email).Result;
                        return role is Role.administrator || role is Role.moderator;
                    });
                });
            });
            return builder;
        }
    }
}
