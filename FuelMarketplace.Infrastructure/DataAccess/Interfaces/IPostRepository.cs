using FuelMarketplace.Domain.Models;
using FuelMarketplace.Shared.Dtos;
using FuelMarketplace.Shared.Dtos.PostCommentDtos;
using FuelMarketplace.Shared.Dtos.PostDtos;

namespace FuelMarketplace.Infrastructure.DataAccess.Interfaces
{
    public interface IPostRepository
    {
        Task AddPostAsync(CreatePostDto dto, int userId, CancellationToken cancellationToken);
        Task<bool> CheckIfPostBelongsToUserAsync(int postId, int userId, CancellationToken cancellationToken);
        Task<bool> CheckIfPostExistsAsync(int postId, CancellationToken cancellationToken);
        Task DeletePostAsync(Post post, CancellationToken cancellationToken);
        Task DeletePostByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<GetPostDto>> GetAllPostsAsync(CancellationToken cancellationToken);
        Task<GetPostDto?> GetPostByIdAsync(int id, CancellationToken cancellationToken);
        Task<UserDto?> GetPostOwnerAsync(int postId, CancellationToken cancellationToken);
        Task<List<GetPostDto>> GetPostsByFuelTypeAsync(FuelType type, CancellationToken cancellationToken);
        Task<List<GetPostDto>> GetPostsByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task UpdatePostAsync(EditPostDto dto, CancellationToken cancellationToken);
        Task AddImageAsync(int postId, Guid fileGuid, CancellationToken cancellationToken);
        Task DeleteImageAsync(int postId, Guid fileGuid, CancellationToken cancellationToken);
        Task AddCommentAsync(int userId, CreatePostCommentDto dto, CancellationToken cancellationToken);
        Task DeleteCommentAsync(int commentId, CancellationToken cancellationToken);
        Task UpdateCommentAsync(EditPostCommentDto dto, CancellationToken cancellationToken);
        Task<bool> CheckIfCommentBelongsToUserAsync(int userId, int commentId, CancellationToken cancellationToken);
        Task<bool> CheckIfCommentExistsAsync(int commentId, CancellationToken cancellationToken);
        Task<List<GetPostCommentDto>> GetCommentsByPostIdAsync(int postId, CancellationToken cancellationToken);
    }
}