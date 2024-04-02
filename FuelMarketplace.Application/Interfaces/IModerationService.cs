using FuelMarketplace.Domain.Models;

namespace FuelMarketplace.Application.Interfaces
{
    public interface IModerationService
    {
        Task BanUserAsync(string email, CancellationToken cancellationToken);
        Task SetUserRoleAsync(string email, Role role, CancellationToken cancellationToken);
        Task UnbanUserAsync(string email, CancellationToken cancellationToken);
    }
}