using FuelMarketplace.Domain.Models;
using FuelMarketplace.Shared.Dtos;
using FuelMarketplace.Shared.Dtos.PostCommentDtos;
using FuelMarketplace.Shared.Dtos.PostDtos;

namespace FuelMarketplace.Application.Interfaces
{
    public interface IPostService
    {
        Task AddPostAsync(CreatePostDto dto, int userId, CancellationToken cancellationToken);
        Task DeletePostAsync(int userId, int postId, CancellationToken cancellationToken, bool moderator = false);
        Task EditPostAsync(int userId, EditPostDto dto, CancellationToken cancellationToken);
        Task<IEnumerable<GetPostDto>> GetAllPostsAsync(CancellationToken cancellationToken);
        Task<GetPostDto> GetPostByIdAsync(int id, CancellationToken cancellationToken);
        Task<UserDto> GetPostOwnerAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<GetPostDto>> GetPostsByFuelTypeAsync(FuelType fuelType, CancellationToken cancellationToken);
        Task<IEnumerable<GetPostDto>> GetPostsByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task AddCommentAsync(int userId, CreatePostCommentDto dto, CancellationToken cancellationToken);
        Task DeleteCommentAsync(int userId, int commentId, CancellationToken cancellationToken, bool moderator = false);
        Task EditCommentAsync(int userId, EditPostCommentDto dto, CancellationToken cancellationToken);
        Task<List<GetPostCommentDto>> GetCommentsByPostIdAsync(int postId, CancellationToken cancellationToken);
    }
}