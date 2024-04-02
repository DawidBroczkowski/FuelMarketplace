using FuelMarketplace.Domain.Models;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using FuelMarketplace.Shared.Dtos.SalesPointDtos;
using Microsoft.EntityFrameworkCore;

namespace FuelMarketplace.Infrastructure.DataAccess
{
    public record DbSalesPointRepository : ISalesPointRepository
    {
        private readonly MarketplaceContext _db;
        public DbSalesPointRepository(MarketplaceContext db)
        {
            _db = db;
        }

        public async Task<GetSalesPointDto?> GetSalesPointByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.SalesPoints
                .IgnoreAutoIncludes()
                .Include(s => s.User)
                .Include(s => s.Images)
                .Select(s => new GetSalesPointDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    ProfileImage = s.ProfileImage,
                    Images = s.Images,
                    Address = s.Address,
                    UserId = s.User!.Id
                })
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<List<GetSalesPointDto>> GetAllSalesPointsAsync(CancellationToken cancellationToken)
        {
            return await _db.SalesPoints
                .IgnoreAutoIncludes()
                .Include(s => s.User)
                .Include(s => s.Images)
                .Select(s => new GetSalesPointDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    ProfileImage = s.ProfileImage,
                    Images = s.Images,
                    Address = s.Address,
                    UserId = s.User!.Id
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetSalesPointDto>> GetSalesPointsByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _db.SalesPoints
                .IgnoreAutoIncludes()
                .Include(s => s.User)
                .Include(s => s.Images)
                .Where(s => s.User!.Id == userId)
                .Select(s => new GetSalesPointDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    ProfileImage = s.ProfileImage,
                    Images = s.Images,
                    Address = s.Address,
                    UserId = s.User!.Id
                })
                .ToListAsync();
        }

        public async Task<GetSalesPointDto?> GetSalesPointByOfferIdAsync(int offerId, CancellationToken cancellationToken)
        {
            return await _db.Offers
                .IgnoreAutoIncludes()
                .Include(o => o.SalesPoint)
                .Where(o => o.Id == offerId)
                .Select(o => new GetSalesPointDto
                {
                    Id = o.SalesPoint!.Id,
                    Name = o.SalesPoint.Name,
                    Description = o.SalesPoint.Description,
                    Address = o.SalesPoint.Address,
                    UserId = o.SalesPoint.User!.Id,
                    Images = o.SalesPoint.Images,
                    ProfileImage = o.SalesPoint.ProfileImage
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddSalesPointAsync(CreateSalesPointDto dto, int userId, CancellationToken cancellationToken)
        {
            var salesPoint = new SalesPoint
            {
                Name = dto.Name,
                Description = dto.Description,
                Address = dto.Address
            };

            _db.SalesPoints.Add(salesPoint);
            var user = await _db.Users.FindAsync(userId);
            user!.SalesPoints.Add(salesPoint);
            _db.Users.Update(user);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateSalesPointAsync(EditSalesPointDto dto, CancellationToken cancellationToken)
        {
            var salesPoint = await _db.SalesPoints.FindAsync(dto.Id);
            salesPoint!.Name = dto.Name ?? salesPoint.Name;
            salesPoint.Description = dto.Description ?? salesPoint.Description;

            _db.SalesPoints.Update(salesPoint);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteSalesPointAsync(SalesPoint salesPoint, CancellationToken cancellationToken)
        {
            _db.SalesPoints.Remove(salesPoint);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteSalesPointByIdAsync(int id, CancellationToken cancellationToken)
        {
            var salesPoint = await _db.SalesPoints.FindAsync(id);

            if (salesPoint != null)
            {
                // Find and update associated offers
                var associatedOffers = await _db.Offers
                    .Where(o => o.SalesPoint!.Id == id)
                    .ToListAsync();

                foreach (var offer in associatedOffers)
                {
                    offer.SalesPoint = null;
                }

                // Remove the sales point
                _db.SalesPoints.Remove(salesPoint);

                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> CheckIfSalesPointExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.SalesPoints
                .IgnoreAutoIncludes()
                .AnyAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<bool> CheckIfSalesPointBelongsToUserAsync(int salesPointId, int userId, CancellationToken cancellationToken)
        {
            return await _db.SalesPoints
                .IgnoreAutoIncludes()
                .Include(s => s.User)
                .AnyAsync(s => s.Id == salesPointId && s.User!.Id == userId, cancellationToken);
        }

        public async Task<bool> CheckIfOfferBelongsToSalesPointAsync(int salesPointId, int offerId, CancellationToken cancellationToken)
        {
            return await _db.Offers
                .IgnoreAutoIncludes()
                .Include(o => o.SalesPoint)
                .AnyAsync(o => o.Id == offerId && o.SalesPoint!.Id == salesPointId, cancellationToken);
        }

        public async Task AddImageAsync(int salesPointId, Guid fileGuid, CancellationToken cancellationToken)
        {
            var salesPoint = await _db.SalesPoints.FindAsync(salesPointId);
            Image image = new Image { FileGuid = fileGuid };
            _db.Images.Add(image);
            salesPoint!.Images.Add(image);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteImageAsync(int salesPointId, Guid fileGuid, CancellationToken cancellationToken)
        {
            var salesPoint = await _db.SalesPoints.FindAsync(salesPointId);
            var image = await _db.Images.FindAsync(fileGuid);
            salesPoint!.Images.Remove(image!);
            _db.Images.Remove(image!);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
