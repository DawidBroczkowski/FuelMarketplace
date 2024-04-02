using FuelMarketplace.Application.Interfaces;
using FuelMarketplace.Domain.Models;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using FuelMarketplace.Shared.Dtos;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using FuelMarketplace.Shared.Extensions;
using FuelMarketplace.Shared.Exceptions;

namespace FuelMarketplace.Application.Services
{
    public class AccountService : IAccountService
    {
        private IUserRepository _repository;
        private IAuthService _authService;
        private IValidationService _validationService;

        public AccountService(IServiceProvider serviceProvider)
        {
            _repository = serviceProvider.GetRequiredService<IUserRepository>();
            _authService = serviceProvider.GetRequiredService<IAuthService>();
            _validationService = serviceProvider.GetRequiredService<IValidationService>();
        }

        public async Task RegisterUserAsync(RegisterDto registerDto, CancellationToken cancellationToken)
        {
            // Check if user with this E-mail already exists
            if (await _repository.CheckIfMailIsTakenAsync(registerDto.Email, cancellationToken))
            {
                var ex = new ValidationException("Can't validate E-mail.");
                ex.Data.Add("Email", "User with that E-mail already exists. ");
                throw ex;
            }

            // Check if E-mail is valid
            if (_validationService.ValidateEmail(registerDto.Email) is false)
            {
                var ex = new ValidationException("Can't validate E-mail.");
                ex.Data.Add("Email", "Invalid email.");
                throw ex;
            }

            // Generate password hash and salt
            _authService.CreatePasswordHash(registerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Add new user to the database
            User user = registerDto.AsUser(passwordHash, passwordSalt);
            user.Role = Role.user;

            await _repository.AddNewUserAsync(user, cancellationToken);
        }

        public async Task<string> LoginUserAsync(LoginDto loginDto, CancellationToken cancellationToken)
        {
            // Get user credentials from the database
            var user = await _repository.GetUserCredentialsAsync(loginDto.Email, cancellationToken);

            // Check if user exists
            if (user is null)
            {
                var ex = new ValidationException("Can't validate E-mail.");
                ex.Data.Add("Email", "User not found.");
                throw ex;
            }

            // Check if user is banned
            if (user.IsBanned)
            {
                var ex = new AuthorizationException("Can't log in.");
                ex.Data.Add("Account", "User is banned.");
                throw ex;
            }

            // Check if password is correct
            if (!_authService.VerifyPasswordHash(loginDto.Password, user.PasswordHash!, user.PasswordSalt!))
            {
                var ex = new ValidationException("Can't validate password.");
                ex.Data.Add("Password", "Wrong password.");
                throw ex;
            }

            // Create and return a JWT token
            string token = _authService.CreateToken(user.Email, user.Id, user.Role);

            return token;
        }
    }
}
