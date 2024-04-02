using FuelMarketplace.Domain.Models;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using FuelMarketplace.Shared.Dtos.OfferCommentDtos;
using FuelMarketplace.Shared.Dtos.PostCommentDtos;
using Microsoft.EntityFrameworkCore;

namespace FuelMarketplace.Infrastructure.DataAccess
{
    public class DbCommentRepository : ICommentRepository
    {
        private readonly MarketplaceContext _db;

        public DbCommentRepository(MarketplaceContext db)
        {
            _db = db;
        }

        public async Task<List<GetPostCommentDto>> GetCommentsByPostIdAsync(int postId, CancellationToken cancellationToken)
        {
            return await _db.PostComments
                .IgnoreAutoIncludes()
                .Include(c => c.User)
                .Where(c => c.Id == postId)
                .Select(c => new GetPostCommentDto
                {
                    Id = c.Id,
                    Description = c.Description,
                    Created = c.Created,
                    UserId = c.User.Id,
                })
                .ToListAsync(cancellationToken);
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
                    UserId = c.User.Id
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<GetPostCommentDto?> GetPostCommentByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.PostComments
                .IgnoreAutoIncludes()
                .Include(c => c.User)
                .Select(c => new GetPostCommentDto
                {
                    Id = c.Id,
                    Description = c.Description,
                    Created = c.Created,
                    UserId = c.User.Id
                })
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<GetOfferCommentDto?> GetOfferCommentByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.OfferComments
                .IgnoreAutoIncludes()
                .Include(c => c.User)
                .Select(c => new GetOfferCommentDto
                {
                    Id = c.Id,
                    Description = c.Description,
                    Created = c.Created,
                    UserId = c.User.Id
                })
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task AddPostCommentAsync(PostComment comment, CancellationToken cancellationToken)
        {
            _db.PostComments.Add(comment);
            var user = await _db.Users.FindAsync(comment.User.Id);
            user!.PostComments.Add(comment);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task AddOfferCommentAsync(OfferComment comment, CancellationToken cancellationToken)
        {
            _db.OfferComments.Add(comment);
            var user = await _db.Users.FindAsync(comment.User.Id);
            user!.OfferComments.Add(comment);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeletePostCommentByIdAsync(int id, CancellationToken cancellationToken)
        {
            var comment = await _db.PostComments.FindAsync(id);
            _db.PostComments.Remove(comment!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteOfferCommentByIdAsync(int id, CancellationToken cancellationToken)
        {
            var comment = await _db.OfferComments.FindAsync(id);
            _db.OfferComments.Remove(comment!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckIfPostCommentExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.PostComments
                .IgnoreAutoIncludes()
                .AnyAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<bool> CheckIfOfferCommentExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.OfferComments
                .IgnoreAutoIncludes()
                .AnyAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<bool> CheckIfPostCommentBelongsToUserAsync(int id, int userId, CancellationToken cancellationToken)
        {
            return await _db.PostComments
                .IgnoreAutoIncludes()
                .Include(c => c.User)
                .AnyAsync(c => c.Id == id && c.User.Id == userId, cancellationToken);
        }

        public async Task<bool> CheckIfOfferCommentBelongsToUserAsync(int id, int userId, CancellationToken cancellationToken)
        {
            return await _db.OfferComments
                .IgnoreAutoIncludes()
                .Include(c => c.User)
                .AnyAsync(c => c.Id == id && c.User.Id == userId, cancellationToken);
        }

        public async Task<bool> CheckIfPostCommentBelongsToUserAsync(int id, string userEmail, CancellationToken cancellationToken)
        {
            return await _db.PostComments
                .IgnoreAutoIncludes()
                .Include(c => c.User)
                .AnyAsync(c => c.Id == id && c.User.Email == userEmail, cancellationToken);
        }

        public async Task<bool> CheckIfOfferCommentBelongsToUserAsync(int id, string userEmail, CancellationToken cancellationToken)
        {
            return await _db.OfferComments
                .IgnoreAutoIncludes()
                .Include(c => c.User)
                .AnyAsync(c => c.Id == id && c.User.Email == userEmail, cancellationToken);
        }

        public async Task AddPostCommentImageAsync(int commentId, Guid fileGuid, CancellationToken cancellationToken)
        {
            var comment = await _db.PostComments.FindAsync(commentId);
            Image image = new Image { FileGuid = fileGuid };
            _db.Images.Add(image);
            comment!.Images.Add(image);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeletePostCommentImageAsync(int commentId, Guid fileGuid, CancellationToken cancellationToken)
        {
            var comment = await _db.PostComments.FindAsync(commentId);
            var image = await _db.Images.FindAsync(fileGuid);
            comment!.Images.Remove(image!);
            _db.Images.Remove(image!);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
