using FuelMarketplace.Domain.Models;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using FuelMarketplace.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading;

namespace FuelMarketplace.Infrastructure.DataAccess
{
    public class DbUserRepository : IUserRepository
    {
        private MarketplaceContext _db;

        public DbUserRepository(MarketplaceContext db)
        {
            _db = db;
        }

        public async Task AddNewUserAsync(User user, CancellationToken cancellationToken)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<UserDto>> GetUserListAsync(CancellationToken cancellationToken)
        {
            List<UserDto> users = await _db.Users
               .IgnoreAutoIncludes()
               .Include(u => u.ProfileImage)
               .Select(u => new UserDto
               {
                   Id = u.Id,
                   Name = u.Name,
                   Surname = u.Surname,
                   Email = u.Email,
                   AccountName = u.AccountName,
                   Role = u.Role,
                   IsBanned = u.IsBanned,
                   Description = u.Description,
                   ProfileImageGuid = u.ProfileImage == null ? Guid.Empty : u.ProfileImage.FileGuid
               })
               .ToListAsync(cancellationToken);

            return users;
        }

        public async Task<UserDto?> GetUserAsync(int id, CancellationToken cancellationToken)
        {
            UserDto? user = await _db.Users
                .IgnoreAutoIncludes()
                .Include(u => u.ProfileImage)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname,
                    Email = u.Email,
                    AccountName = u.AccountName,
                    Role = u.Role,
                    IsBanned = u.IsBanned,
                    Description = u.Description,
                    ProfileImageGuid = u.ProfileImage == null ? Guid.Empty : u.ProfileImage.FileGuid
                })
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            return user;
        }

        public async Task<UserDto?> GetUserAsync(string email, CancellationToken cancellationToken)
        {
            UserDto? user = await _db.Users
                .IgnoreAutoIncludes()
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname,
                    Email = u.Email,
                    AccountName = u.AccountName,
                    Role = u.Role,
                    IsBanned = u.IsBanned,
                    Description = u.Description,
                    ProfileImageGuid = u.ProfileImage == null ? Guid.Empty : u.ProfileImage.FileGuid
                })
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
            return user;
        }

        public async Task<int> GetUserIdByEmail(string email, CancellationToken cancellationToken)
        {
            return await _db.Users
                .IgnoreAutoIncludes()
                .Where(u => u.Email == email)
                .Select(u => u.Id)
                .FirstAsync();
        }

        public async Task<bool> CheckIfMailIsTakenAsync(string mail, CancellationToken cancellationToken)
        {
            bool taken = await _db.Users
                .IgnoreAutoIncludes()
                .AnyAsync(u => u.Email == mail, cancellationToken);
            return taken;
        }

        public async Task<CredentialsDto?> GetUserCredentialsAsync(string email, CancellationToken cancellationToken)
        {
            CredentialsDto? credentials = await _db.Users
                .IgnoreAutoIncludes()
                .Where(u => u.Email == email)
                .Select(u =>
                new CredentialsDto
                {
                    Id = u.Id,
                    Email = email,
                    PasswordHash = u.PasswordHash,
                    PasswordSalt = u.PasswordSalt,
                    Role = u.Role,
                    IsBanned = u.IsBanned
                })
                .FirstOrDefaultAsync(cancellationToken);

            return credentials;
        }

        public async Task<CredentialsDto?> GetUserCredentialsAsync(int id, CancellationToken cancellationToken)
        {
            CredentialsDto? credentials = await _db.Users
                .IgnoreAutoIncludes()
                .Where(u => u.Id == id)
                .Select(u =>
                new CredentialsDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    PasswordHash = u.PasswordHash,
                    PasswordSalt = u.PasswordSalt,
                    Role = u.Role,
                    IsBanned = u.IsBanned
                })
                .FirstOrDefaultAsync(cancellationToken);

            return credentials;
        }

        public async Task<bool> GetUserBanStatusAsync(string email, CancellationToken cancellationToken)
        {
            bool isBanned = await _db.Users
                .IgnoreAutoIncludes()
                .Where(u => u.Email == email)
                .Select(u => u.IsBanned)
                .FirstAsync(cancellationToken);

            return isBanned;
        }

        public async Task<bool> GetUserBanStatusAsync(string email)
        {
            bool isBanned = await _db.Users
                .IgnoreAutoIncludes()
                .Where(u => u.Email == email)
                .Select(u => u.IsBanned)
                .FirstAsync();

            return isBanned;
        }

        public async Task<bool> GetUserBanStatusAsync(int id, CancellationToken cancellationToken)
        {
            bool isBanned = await _db.Users
                .IgnoreAutoIncludes()
                .Where(u => u.Id == id)
                .Select(u => u.IsBanned)
                .FirstAsync(cancellationToken);

            return isBanned;
        }

        public async Task SetUserRoleAsync(string email, Role role, CancellationToken cancellationToken)
        {
            var user = await _db.Users
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
            user!.Role = role;
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task SetUserRoleAsync(int id, Role role, CancellationToken cancellationToken)
        {
            var user = await _db.Users
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            user!.Role = role;
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<Role> GetUserRoleAsync(string email, CancellationToken cancellationToken)
        {
            Role role = await _db.Users
                .IgnoreAutoIncludes()
                .Where(u => u.Email == email)
                .Select(u => u.Role)
                .FirstAsync(cancellationToken);

            return role;
        }

        public async Task<Role> GetUserRoleAsync(string email)
        {
            Role role = await _db.Users
                .IgnoreAutoIncludes()
                .Where(u => u.Email == email)
                .Select(u => u.Role)
                .FirstAsync();

            return role;
        }

        public async Task<Role> GetUserRoleAsync(int id, CancellationToken cancellationToken)
        {
            Role role = await _db.Users
                .IgnoreAutoIncludes()
                .Where(u => u.Id == id)
                .Select(u => u.Role)
                .FirstAsync(cancellationToken);

            return role;
        }

        public async Task SetUserBanAsync(string email, bool isBanned, CancellationToken cancellationToken)
        {
            var user = await _db.Users
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
            user!.IsBanned = isBanned;
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task SetUserBanAsync(int id, bool isBanned, CancellationToken cancellationToken)
        {
            var user = await _db.Users
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            user!.IsBanned = isBanned;
            await _db.SaveChangesAsync(cancellationToken);
        }

        // Todo: Change to dto
        public async Task UpdateUserAsync(EditUserDto dto, CancellationToken cancellationToken)
        {
            var user = await _db.Users
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(u => u.Id == dto.Id, cancellationToken);

            user!.Description = dto.Description ?? user.Description;

            if (dto.ProfileImageGuid is not null && dto.ProfileImageGuid != Guid.Empty)
            {
                Image image = new Image
                {
                    FileGuid = (Guid)dto.ProfileImageGuid
                };
                _db.Images.Add(image);

                user.ProfileImage = image;
            }

            _db.Users.Update(user);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task ChangeProfileImageAsync(string email, Image? image, CancellationToken cancellationToken)
        {
            var user = await _db.Users
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
            user!.ProfileImage = image;
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckIfUserExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.Users
                .IgnoreAutoIncludes()
                .AnyAsync(u => u.Id == id, cancellationToken);
        }
    }
}
