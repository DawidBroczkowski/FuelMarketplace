using FuelMarketplace.Domain.Models;
using FuelMarketplace.Shared.Dtos;

namespace FuelMarketplace.Infrastructure.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task AddNewUserAsync(User user, CancellationToken cancellationToken);
        Task ChangeProfileImageAsync(string email, Image? image, CancellationToken cancellationToken);
        Task<bool> CheckIfMailIsTakenAsync(string mail, CancellationToken cancellationToken);
        Task<bool> CheckIfUserExistsAsync(int id, CancellationToken cancellationToken);
        Task<UserDto?> GetUserAsync(int id, CancellationToken cancellationToken);
        Task<UserDto?> GetUserAsync(string email, CancellationToken cancellationToken);
        Task<bool> GetUserBanStatusAsync(int id, CancellationToken cancellationToken);
        Task<bool> GetUserBanStatusAsync(string email, CancellationToken cancellationToken);
        Task<bool> GetUserBanStatusAsync(string email);
        Task<CredentialsDto?> GetUserCredentialsAsync(int id, CancellationToken cancellationToken);
        Task<CredentialsDto?> GetUserCredentialsAsync(string email, CancellationToken cancellationToken);
        Task<int> GetUserIdByEmail(string email, CancellationToken cancellationToken);
        Task<List<UserDto>> GetUserListAsync(CancellationToken cancellationToken);
        Task<Role> GetUserRoleAsync(int id, CancellationToken cancellationToken);
        Task<Role> GetUserRoleAsync(string email, CancellationToken cancellationToken);
        Task<Role> GetUserRoleAsync(string email);
        Task SetUserBanAsync(int id, bool isBanned, CancellationToken cancellationToken);
        Task SetUserBanAsync(string email, bool isBanned, CancellationToken cancellationToken);
        Task SetUserRoleAsync(int id, Role role, CancellationToken cancellationToken);
        Task SetUserRoleAsync(string email, Role role, CancellationToken cancellationToken);
        Task UpdateUserAsync(EditUserDto user, CancellationToken cancellationToken);
    }
}