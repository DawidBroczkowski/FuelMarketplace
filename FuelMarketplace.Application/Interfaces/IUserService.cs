using FuelMarketplace.Shared.Dtos;

namespace FuelMarketplace.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetLoggedUserAsync(string email, CancellationToken cancellationToken);
        Task<IEnumerable<UserDto>> GetUserListAsync(CancellationToken cancellationToken);
        Task<UserDto> GetUserAsync(int id, CancellationToken cancellationToken);
        Task<UserDto> GetUserAsync(string email, CancellationToken cancellationToken);
        Task UpdateUserAsync(EditUserDto dto, CancellationToken cancellationToken);
    }
}