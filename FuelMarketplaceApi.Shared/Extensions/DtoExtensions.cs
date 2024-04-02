using FuelMarketplace.Domain.Models;
using FuelMarketplace.Shared.Dtos;

namespace FuelMarketplace.Shared.Extensions
{
    public static class DtoExtensions
    {
        public static User AsUser(this RegisterDto registerDto, byte[] passwordHash, byte[] passwordSalt)
        {
            return new User()
            {
                Email = registerDto.Email,
                Name = registerDto.Name,
                Surname = registerDto.Surname,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                AccountName = registerDto.AccountName
            };
        }
    }
}
