using FuelMarketplace.Shared.Dtos;

namespace FuelMarketplace.Application.Interfaces
{
    public interface IAccountService
    {
        Task<string> LoginUserAsync(LoginDto loginDto, CancellationToken cancellationToken);
        Task RegisterUserAsync(RegisterDto registerDto, CancellationToken cancellationToken);
    }
}