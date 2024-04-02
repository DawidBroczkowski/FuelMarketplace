using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using FuelMarketplace.Shared.Dtos;
using FuelMarketplace.Domain.Models;
using FuelMarketplace.Infrastructure.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using FuelMarketplace.Shared.Exceptions;
using FuelMarketplace.Application.Interfaces;
using FuelMarketplace.Shared.Dtos.PostDtos;
using FuelMarketplace.Shared.Dtos.PostCommentDtos;

namespace FuelMarketplace.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public PostService(IPostRepository postRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public async Task AddPostAsync(CreatePostDto dto, int userId, CancellationToken cancellationToken)
        {
            await _postRepository.AddPostAsync(dto, userId, cancellationToken);
        }

        public async Task<GetPostDto> GetPostByIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _postRepository.GetPostByIdAsync(id, cancellationToken);
            if (result is null)
            {
                var ex = new KeyNotFoundException("Can't find post.");
                ex.Data.Add("Id", "Post not found.");
                throw ex;
            }
            return result;
        }

        public async Task<IEnumerable<GetPostDto>> GetAllPostsAsync(CancellationToken cancellationToken)
        {
            return await _postRepository.GetAllPostsAsync(cancellationToken);
        }

        public async Task DeletePostAsync(int userId, int postId, CancellationToken cancellationToken, bool moderator = false)
        {
            var postExists = await _postRepository.CheckIfPostExistsAsync(postId, cancellationToken);
            if (postExists is false)
            {
                var ex = new KeyNotFoundException("Can't find post.");
                ex.Data.Add("Id", "Post not found.");
                throw ex;
            }
            if (await _postRepository.CheckIfPostBelongsToUserAsync(postId, userId, cancellationToken) is false
                               && moderator is false)
            {
                var ex = new AuthorizationException("User is not owner.");
                ex.Data.Add("User", "User is not the owner of this post.");
                throw ex;
            }
            await _postRepository.DeletePostByIdAsync(postId, cancellationToken);
        }

        public async Task<IEnumerable<GetPostDto>> GetPostsByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var userExists = await _userRepository.CheckIfUserExistsAsync(userId, cancellationToken);
            if (userExists is false)
            {
                var ex = new KeyNotFoundException("Can't find user.");
                ex.Data.Add("Id", "User not found.");
                throw ex;
            }
            return await _postRepository.GetPostsByUserIdAsync(userId, cancellationToken);
        }

        public async Task<IEnumerable<GetPostDto>> GetPostsByFuelTypeAsync(FuelType fuelType, CancellationToken cancellationToken)
        {
            return await _postRepository.GetPostsByFuelTypeAsync(fuelType, cancellationToken);
        }

        public async Task EditPostAsync(int userId, EditPostDto dto, CancellationToken cancellationToken)
        {
            var postExists = await _postRepository.CheckIfPostExistsAsync(dto.Id, cancellationToken);
            if (postExists is false)
            {
                var ex = new KeyNotFoundException("Can't find post.");
                ex.Data.Add("Id", "Post not found.");
                throw ex;
            }
            if (await _postRepository.CheckIfPostBelongsToUserAsync(dto.Id, userId, cancellationToken) is false)
            {
                var ex = new AuthorizationException("User is not owner.");
                ex.Data.Add("User", "User is not the owner of this post.");
                throw ex;
            }
            await _postRepository.UpdatePostAsync(dto, cancellationToken);
        }

        public async Task<UserDto> GetPostOwnerAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _postRepository.GetPostOwnerAsync(id, cancellationToken);
            if (result is null)
            {
                var ex = new KeyNotFoundException("Can't find post.");
                ex.Data.Add("Id", "Post not found.");
                throw ex;
            }
            return result;
        }

        public async Task AddCommentAsync(int userId, CreatePostCommentDto dto, CancellationToken cancellationToken)
        {
            var postExists = await _postRepository.CheckIfPostExistsAsync(dto.PostId, cancellationToken);
            if (postExists is false)
            {
                var ex = new KeyNotFoundException("Can't find post.");
                ex.Data.Add("Id", "Post not found.");
                throw ex;
            }
            await _postRepository.AddCommentAsync(userId, dto, cancellationToken);
        }

        public async Task DeleteCommentAsync(int userId, int commentId, CancellationToken cancellationToken, bool moderator = false)
        {
            var commentExists = await _postRepository.CheckIfCommentExistsAsync(commentId, cancellationToken);
            if (commentExists is false)
            {
                var ex = new KeyNotFoundException("Can't find comment.");
                ex.Data.Add("Id", "Comment not found.");
                throw ex;
            }
            if (await _postRepository.CheckIfCommentBelongsToUserAsync(userId, commentId, cancellationToken) is false 
                && moderator is false)
            {
                var ex = new AuthorizationException("User is not owner.");
                ex.Data.Add("User", "User is not the owner of this comment.");
                throw ex;
            }
            await _postRepository.DeleteCommentAsync(commentId, cancellationToken);
        }

        public async Task EditCommentAsync(int userId, EditPostCommentDto dto, CancellationToken cancellationToken)
        {
            var commentExists = await _postRepository.CheckIfCommentExistsAsync(dto.Id, cancellationToken);
            if (commentExists is false)
            {
                var ex = new KeyNotFoundException("Can't find comment.");
                ex.Data.Add("Id", "Comment not found.");
                throw ex;
            }
            if (await _postRepository.CheckIfCommentBelongsToUserAsync(dto.Id, userId, cancellationToken) is false)
            {
                var ex = new AuthorizationException("User is not owner.");
                ex.Data.Add("User", "User is not the owner of this comment.");
                throw ex;
            }
            await _postRepository.UpdateCommentAsync(dto, cancellationToken);
        }

        public async Task<List<GetPostCommentDto>> GetCommentsByPostIdAsync(int postId, CancellationToken cancellationToken)
        {
            var postExists = await _postRepository.CheckIfPostExistsAsync(postId, cancellationToken);
            if (postExists is false)
            {
                var ex = new KeyNotFoundException("Can't find post.");
                ex.Data.Add("Id", "Post not found.");
                throw ex;
            }
            return await _postRepository.GetCommentsByPostIdAsync(postId, cancellationToken);
        }
    }
}
