using FuelMarketplace.Application.Interfaces;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using FuelMarketplace.Shared.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace FuelMarketplace.Application.Services
{
    public class UserService : IUserService
    {
        private IServiceProvider _serviceProvider;
        private IUserRepository _repository;

        public UserService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _repository = _serviceProvider.GetRequiredService<IUserRepository>();
        }

        public async Task<IEnumerable<UserDto>> GetUserListAsync(CancellationToken cancellationToken)
        {
            var users = await _repository.GetUserListAsync(cancellationToken);
            return users;
        }

        public async Task<UserDto> GetLoggedUserAsync(string email, CancellationToken cancellationToken)
        {
            UserDto user = (await _repository.GetUserAsync(email, cancellationToken))!;
            return user;
        }

        public async Task<UserDto> GetUserAsync(int id, CancellationToken cancellationToken)
        {
            UserDto user = (await _repository.GetUserAsync(id, cancellationToken))!;
            return user;
        }

        public async Task<UserDto> GetUserAsync(string email, CancellationToken cancellationToken)
        {
            UserDto user = (await _repository.GetUserAsync(email, cancellationToken))!;
            return user;
        }

        public async Task UpdateUserAsync(EditUserDto dto, CancellationToken cancellationToken)
        {
            await _repository.UpdateUserAsync(dto, cancellationToken);
        }
    }
}
