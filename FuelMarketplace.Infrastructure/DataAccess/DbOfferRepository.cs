using FuelMarketplace.Domain.Models;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using FuelMarketplace.Shared.Dtos;
using FuelMarketplace.Shared.Dtos.OfferCommentDtos;
using FuelMarketplace.Shared.Dtos.OfferDtos;
using Microsoft.EntityFrameworkCore;

namespace FuelMarketplace.Infrastructure.DataAccess
{
    public class DbOfferRepository : IOfferRepository
    {
        private readonly MarketplaceContext _db;

        public DbOfferRepository(MarketplaceContext db)
        {
            _db = db;
        }

        public async Task<GetOfferDto?> GetOfferByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.Offers
                .IgnoreAutoIncludes()
                .Include(o => o.User)
                .Include(o => o.Images)
                .Include(o => o.SalesPoint)
                .Select(o => new GetOfferDto
                {
                    Id = o.Id,
                    Title = o.Title,
                    Description = o.Description,
                    Price = o.Price,
                    FuelType = o.FuelType,
                    SalesPointId = o.SalesPoint!.Id,
                    UserId = o.User.Id,
                    Images = o.Images,
                    Address = o.Address,
                    Created = o.Created,
                    Updated = o.Updated
                })
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<List<GetOfferDto>> GetAllOffersAsync(CancellationToken cancellationToken)
        {
            return await _db.Offers
                .IgnoreAutoIncludes()
                .Include(o => o.User)
                .Include(o => o.Images)
                .Include(o => o.SalesPoint)
                .Select(o => new GetOfferDto
                {
                    Id = o.Id,
                    Title = o.Title,
                    Description = o.Description,
                    Price = o.Price,
                    FuelType = o.FuelType,
                    SalesPointId = o.SalesPoint!.Id,
                    UserId = o.User.Id,
                    Images = o.Images,
                    Address = o.Address,
                    Created = o.Created,
                    Updated = o.Updated
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetOfferDto>> GetOffersByFuelTypeAsync(FuelType type, CancellationToken cancellationToken)
        {
            return await _db.Offers
                .IgnoreAutoIncludes()
                .Include(o => o.User)
                .Include(o => o.Images)
                .Include(o => o.SalesPoint)
                .Where(o => o.FuelType == type)
                .Select(o => new GetOfferDto
                {
                    Id = o.Id,
                    Title = o.Title,
                    Description = o.Description,
                    Price = o.Price,
                    FuelType = o.FuelType,
                    SalesPointId = o.SalesPoint!.Id,
                    UserId = o.User.Id,
                    Images = o.Images,
                    Address = o.Address
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetOfferDto>> GetOffersBySalesPointIdAsync(int salesPointId, CancellationToken cancellationToken)
        {
            return await _db.Offers
                .IgnoreAutoIncludes()
                .Include(o => o.User)
                .Include(o => o.Images)
                .Include(o => o.SalesPoint)
                .Where(o => o.SalesPoint.Id == salesPointId)
                .Select(o => new GetOfferDto
                {
                    Id = o.Id,
                    Title = o.Title,
                    Description = o.Description,
                    Price = o.Price,
                    FuelType = o.FuelType,
                    SalesPointId = o.SalesPoint!.Id,
                    UserId = o.User.Id,
                    Images = o.Images,
                    Address = o.Address
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetOfferDto>> GetOffersByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _db.Offers
                .IgnoreAutoIncludes()
                .Include(o => o.User)
                .Include(o => o.Images)
                .Include(o => o.SalesPoint)
                .Where(o => o.User.Id == userId)
                .Select(o => new GetOfferDto
                {
                    Id = o.Id,
                    Title = o.Title,
                    Description = o.Description,
                    Price = o.Price,
                    FuelType = o.FuelType,
                    SalesPointId = o.SalesPoint!.Id,
                    UserId = o.User.Id,
                    Images = o.Images,
                    Address = o.Address
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<UserDto?> GetOfferOwnerAsync(int offerId, CancellationToken cancellationToken)
        {
            return await _db.Offers
                .IgnoreAutoIncludes()
                .Include(o => o.User)
                .Where(o => o.Id == offerId)
                .Select(o => new UserDto
                {
                    Id = o.Id,
                    Name = o.User.Name,
                    Surname = o.User.Surname,
                    Email = o.User.Email,
                    AccountName = o.User.AccountName,
                    Role = o.User.Role,
                    IsBanned = o.User.IsBanned,
                    ProfileImageGuid = o.User.ProfileImage!.FileGuid
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddOfferAsync(CreateOfferDto dto, int userId, CancellationToken cancellationToken)
        {
            var offer = new Offer
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                FuelType = dto.FuelType,
                Created = DateTime.Now,
                Address = dto.Address
            };

            _db.Offers.Add(offer);
            var user = await _db.Users.FindAsync(userId);
            user!.Offers.Add(offer);

            if (dto.SalesPointId is not null)
            {
                var salesPoint = await _db.SalesPoints.FindAsync(dto.SalesPointId);
                if (salesPoint is not null)
                {
                    salesPoint.Offers.Add(offer);
                }
            }

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateOfferAsync(EditOfferDto dto, CancellationToken cancellationToken)
        {
            var offer = await _db.Offers.FindAsync(dto.Id);

            offer!.Title = dto.Title ?? offer!.Title;
            offer.Description = dto.Description ?? offer!.Description;
            offer.Price = dto.Price ?? offer!.Price;
            offer.FuelType = dto.FuelType ?? offer!.FuelType;
            offer.Updated = DateTime.Now;
            
            if (dto.SalesPointId is not null)
            {
                var salesPoint = await _db.SalesPoints.FindAsync(dto.SalesPointId);
                offer.SalesPoint = salesPoint;
            }

            _db.Offers.Update(offer!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteOfferAsync(Offer offer, CancellationToken cancellationToken)
        {
            _db.Offers.Remove(offer);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteOfferByIdAsync(int offerId, CancellationToken cancellationToken)
        {
            var offer = await _db.Offers.FindAsync(offerId);
            offer!.Comments.Clear();
            _db.Offers.Remove(offer!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckIfOfferExistsAsync(int offerId, CancellationToken cancellationToken)
        {
            return await _db.Offers
                .IgnoreAutoIncludes()
                .AnyAsync(o => o.Id == offerId, cancellationToken);
        }

        public async Task<bool> CheckIfOfferBelongsToUserAsync(int offerId, int userId, CancellationToken cancellationToken)
        {
            return await _db.Offers
                .IgnoreAutoIncludes()
                .Include(o => o.User)
                .AnyAsync(o => o.Id == offerId && o.User.Id == userId, cancellationToken);
        }

        public async Task<bool> CheckIfOfferBelongsToUserAsync(int offerId, string userEmail, CancellationToken cancellationToken)
        {
            return await _db.Offers
                .IgnoreAutoIncludes()
                .Include(o => o.User)
                .AnyAsync(o => o.User.Email == userEmail && o.Id == offerId, cancellationToken);
        }

        public async Task<bool> CheckIfOfferBelongsToSalesPointAsync(int offerId, int salesPointId, CancellationToken cancellationToken)
        {
            return await _db.Offers
                .IgnoreAutoIncludes()
                .Include(o => o.SalesPoint)
                .AnyAsync(o => o.Id == offerId && o.SalesPoint!.Id == salesPointId, cancellationToken);
        }

        public async Task AddImageAsync(int offerId, Guid fileGuid, CancellationToken cancellationToken)
        {
            var offer = await _db.Offers.FindAsync(offerId);
            Image image = new Image { FileGuid = fileGuid };
            _db.Images.Add(image);
            offer!.Images.Add(image);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteImageAsync(int offerId, Guid fileGuid, CancellationToken cancellationToken)
        {
            var offer = await _db.Offers.FindAsync(offerId);
            var image = await _db.Images.FindAsync(fileGuid);
            offer!.Images.Remove(image!);
            _db.Images.Remove(image!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task AddCommentAsync(int userId, CreateOfferCommentDto dto, CancellationToken cancellationToken)
        {
            var offer = await _db.Offers.FindAsync(dto.OfferId);
            var user = await _db.Users.FindAsync(userId);
            var comment = new OfferComment
            {
                Description = dto.Description,
                Created = DateTime.Now
            };
            _db.OfferComments.Add(comment);
            offer!.Comments.Add(comment);
            user!.OfferComments.Add(comment);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCommentAsync(int commentId, CancellationToken cancellationToken)
        {
            var offer = await _db.Offers
                .IgnoreAutoIncludes()
                .Include(o => o.Comments)
                .FirstOrDefaultAsync(o => o.Comments.Any(c => c.Id == commentId), cancellationToken);
            var comment = offer!.Comments.FirstOrDefault(c => c.Id == commentId);
            offer!.Comments.Remove(comment!);
            _db.OfferComments.Remove(comment!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateCommentAsync(EditOfferCommentDto dto, CancellationToken cancellationToken)
        {
            var comment = await _db.OfferComments.FindAsync(dto.Id);
            comment!.Updated = DateTime.Now;
            comment!.Description = comment.Description;
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckIfCommentBelongsToUserAsync(int userId, int commentId, CancellationToken cancellationToken)
        {
            return await _db.OfferComments
                .IgnoreAutoIncludes()
                .Include(c => c.User)
                .AnyAsync(c => c.Id == commentId && c.User.Id == userId, cancellationToken);
        }

        public async Task<bool> CheckIfCommentExistsAsync(int commentId, CancellationToken cancellationToken)
        {
            return await _db.OfferComments
                .IgnoreAutoIncludes()
                .Include(c => c.User)
                .AnyAsync(c => c.Id == commentId, cancellationToken);
        }

        public async Task<List<GetOfferCommentDto>> GetCommentsByOfferIdAsync(int offerId, CancellationToken cancellationToken)
        {
            return await _db.OfferComments
                .IgnoreAutoIncludes()
                .Include(c => c.User)
                .Where(c => c.Offer.Id == offerId)
                .Select(c => new GetOfferCommentDto
                {
                    Id = c.Id,
                    Description = c.Description,
                    Created = c.Created,
                    Updated = c.Updated,
                    UserId = c.User.Id
                })
                .ToListAsync(cancellationToken);
        }
    }
}
