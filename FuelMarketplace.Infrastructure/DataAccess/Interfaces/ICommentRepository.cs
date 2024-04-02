using FuelMarketplace.Domain.Models;
using FuelMarketplace.Shared.Dtos.OfferCommentDtos;
using FuelMarketplace.Shared.Dtos.PostCommentDtos;

namespace FuelMarketplace.Infrastructure.DataAccess.Interfaces
{
    public interface ICommentRepository
    {
        Task AddOfferCommentAsync(OfferComment comment, CancellationToken cancellationToken);
        Task AddPostCommentAsync(PostComment comment, CancellationToken cancellationToken);
        Task<bool> CheckIfOfferCommentExistsAsync(int id, CancellationToken cancellationToken);
        Task<bool> CheckIfPostCommentExistsAsync(int id, CancellationToken cancellationToken);
        Task DeleteOfferCommentByIdAsync(int id, CancellationToken cancellationToken);
        Task DeletePostCommentByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<GetOfferCommentDto>> GetCommentsByOfferIdAsync(int offerId, CancellationToken cancellationToken);
        Task<List<GetPostCommentDto>> GetCommentsByPostIdAsync(int postId, CancellationToken cancellationToken);
        Task<GetOfferCommentDto?> GetOfferCommentByIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> CheckIfOfferCommentBelongsToUserAsync(int id, int userId, CancellationToken cancellationToken);
        Task<bool> CheckIfPostCommentBelongsToUserAsync(int id, int userId, CancellationToken cancellationToken);
        Task<bool> CheckIfOfferCommentBelongsToUserAsync(int id, string userEmail, CancellationToken cancellationToken);
        Task<bool> CheckIfPostCommentBelongsToUserAsync(int id, string userEmail, CancellationToken cancellationToken);
        Task AddPostCommentImageAsync(int commentId, Guid fileGuid, CancellationToken cancellationToken);
        Task DeletePostCommentImageAsync(int commentId, Guid fileGuid, CancellationToken cancellationToken);


    }
}