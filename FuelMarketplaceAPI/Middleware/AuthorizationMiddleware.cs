using FuelMarketplace.Domain.Models;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using FuelMarketplace.Shared.Exceptions;
using System.Security.Claims;

namespace FuelMarketplace.API.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserRepository userRepository)
        {
            // Check if the user is banned
            if (context.User.Identity!.IsAuthenticated)
            {
                string email = context.User.FindFirstValue(ClaimTypes.Email)!;
                bool isBanned = await userRepository.GetUserBanStatusAsync(email);

                if (isBanned)
                {
                    // If the user is banned, throw an exception and stop the request
                    var ex = new AuthorizationException("Can't execute the request.");
                    ex.Data.Add("Account", "User is banned.");
                    throw ex;
                }
            }

            // Continue processing the request
            await _next(context);
        }
    }
}
