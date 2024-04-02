using FuelMarketplace.Application.Interfaces;
using FuelMarketplace.Domain.Models;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using FuelMarketplace.Shared.Dtos;
using FuelMarketplace.Shared.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace FuelMarketplace.Application.Services
{
    public class ModerationService : IModerationService
    {
        private IUserRepository _repository;
        private IServiceProvider _serviceProvider;

        public ModerationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _repository = _serviceProvider.GetRequiredService<IUserRepository>();
        }

        public async Task BanUserAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserAsync(email, cancellationToken);
            checkIfUserExists(user);

            var role = await _repository.GetUserRoleAsync(email, cancellationToken);

            if (role > 0)
            {
                if (user is null)
                {
                    var ex = new AuthorizationException("Can't execute the request.");
                    ex.Data.Add("Role", "User is a member of staff. Change the role to \"user\" before banning.");
                    throw ex;
                }
            }

            await _repository.SetUserBanAsync(email, true, cancellationToken);
        }

        public async Task UnbanUserAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserAsync(email, cancellationToken);
            checkIfUserExists(user);

            await _repository.SetUserBanAsync(email, false, cancellationToken);
        }

        public async Task SetUserRoleAsync(string email, Role role, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserAsync(email, cancellationToken);
            checkIfUserExists(user);

            await _repository.SetUserRoleAsync(email, role, cancellationToken);
        }

        private void checkIfUserExists(UserDto? user)
        {
            if (user is null)
            {
                var ex = new ValidationException("Can't validate E-mail.");
                ex.Data.Add("Email", "User not found.");
                throw ex;
            }
        }
    }
}
