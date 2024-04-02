using FuelMarketplace.Domain.Models;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using FuelMarketplace.Shared.Dtos;
using FuelMarketplace.Shared.Dtos.PostCommentDtos;
using FuelMarketplace.Shared.Dtos.PostDtos;
using Microsoft.EntityFrameworkCore;
using System;

namespace FuelMarketplace.Infrastructure.DataAccess
{
    public class DbPostRepository : IPostRepository
    {
        private readonly MarketplaceContext _db;

        public DbPostRepository(MarketplaceContext db)
        {
            _db = db;
        }

        public async Task<GetPostDto?> GetPostByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.Posts
                .IgnoreAutoIncludes()
                .Include(p => p.User)
                .Include(p => p.Images)
                .Select(p => new GetPostDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    UserId = p.User.Id,
                    Images = p.Images,
                    Created = p.Created,
                    Updated = p.Updated,
                    FuelType = p.FuelType,
                    Address = p.Address
                })
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<GetPostDto>> GetAllPostsAsync(CancellationToken cancellationToken)
        {
            return await _db.Posts
                .IgnoreAutoIncludes()
                .Include(p => p.User)
                .Include(p => p.Images)
                .Select(p => new GetPostDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    UserId = p.User.Id,
                    Images = p.Images,
                    Created = p.Created,
                    Updated = p.Updated,
                    FuelType = p.FuelType,
                    Address = p.Address
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetPostDto>> GetPostsByFuelTypeAsync(FuelType type, CancellationToken cancellationToken)
        {
            return await _db.Posts
                .IgnoreAutoIncludes()
                .Include(p => p.User)
                .Include(p => p.Images)
                .Where(p => p.FuelType == type)
                .Select(p => new GetPostDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    UserId = p.User.Id,
                    Images = p.Images,
                    Created = p.Created,
                    Updated = p.Updated,
                    FuelType = p.FuelType,
                    Address = p.Address
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GetPostDto>> GetPostsByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _db.Posts
                .IgnoreAutoIncludes()
                .Include(p => p.User)
                .Include(p => p.Images)
                .Where(p => p.User.Id == userId)
                .Select(p => new GetPostDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    UserId = p.User.Id,
                    Images = p.Images,
                    Created = p.Created,
                    Updated = p.Updated,
                    FuelType = p.FuelType,
                    Address = p.Address
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<UserDto?> GetPostOwnerAsync(int postId, CancellationToken cancellationToken)
        {
            return await _db.Posts
                .IgnoreAutoIncludes()
                .Include(p => p.User)
                .Where(p => p.Id == postId)
                .Select(p => new UserDto
                {
                    Id = p.User.Id,
                    Name = p.User.Name,
                    Surname = p.User.Surname,
                    Email = p.User.Email,
                    AccountName = p.User.AccountName,
                    Role = p.User.Role,
                    IsBanned = p.User.IsBanned,
                    ProfileImageGuid = p.User.ProfileImage!.FileGuid
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddPostAsync(CreatePostDto dto, int userId, CancellationToken cancellationToken)
        {
            Post post = new Post
            {
                FuelType = dto.FuelType,
                Title = dto.Title,
                Description = dto.Description,
                Address = dto.Address,
                Created = DateTime.Now
            };

            _db.Posts.Add(post);
            var user = await _db.Users.FindAsync(userId);
            user!.Posts.Add(post);

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdatePostAsync(EditPostDto dto, CancellationToken cancellationToken)
        {
            var post = await _db.Posts.FindAsync(dto.Id);

            post!.Title = dto.Title ?? post.Title;
            post.Description = dto.Description ?? post.Description;
            post.FuelType = dto.FuelType ?? post.FuelType;
            post.Address = dto.Address ?? post.Address;
            post.Updated = DateTime.Now;

            _db.Posts.Update(post);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeletePostAsync(Post post, CancellationToken cancellationToken)
        {
            _db.Posts.Remove(post);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeletePostByIdAsync(int postId, CancellationToken cancellationToken)
        {
            var post = await _db.Posts.FindAsync(postId);
            if (post!.Comments is not null)
            {
                post!.Comments.Clear();
            }
            _db.Posts.Remove(post!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckIfPostExistsAsync(int postId, CancellationToken cancellationToken)
        {
            return await _db.Posts
                .IgnoreAutoIncludes()
                .AnyAsync(p => p.Id == postId, cancellationToken);
        }

        public async Task<bool> CheckIfPostBelongsToUserAsync(int postId, int userId, CancellationToken cancellationToken)
        {
            return await _db.Posts
                .IgnoreAutoIncludes()
                .Include(p => p.User)
                .AnyAsync(p => p.Id == postId && p.User.Id == userId, cancellationToken);
        }

        public async Task AddImageAsync(int postId, Guid fileGuid, CancellationToken cancellationToken)
        {
            var post = await _db.Posts.FindAsync(postId);
            Image image = new Image { FileGuid = fileGuid };
            _db.Images.Add(image);
            post!.Images.Add(image);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteImageAsync(int postId, Guid fileGuid, CancellationToken cancellationToken)
        {
            var post = await _db.Posts.FindAsync(postId);
            var image = await _db.Images.FindAsync(fileGuid);
            post!.Images.Remove(image!);
            _db.Images.Remove(image!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task AddCommentAsync(int userId, CreatePostCommentDto dto, CancellationToken cancellationToken)
        {
            var post = await _db.Posts.FindAsync(dto.PostId);
            var user = await _db.Users.FindAsync(userId);
            PostComment comment = new PostComment
            {
                Description = dto.Description,
                Created = DateTime.Now
            };
            _db.PostComments.Add(comment);
            post!.Comments.Add(comment);
            user!.PostComments.Add(comment);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCommentAsync(int commentId, CancellationToken cancellationToken)
        {
            var comment = await _db.PostComments.FindAsync(commentId);
            _db.PostComments.Remove(comment!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateCommentAsync(EditPostCommentDto dto, CancellationToken cancellationToken)
        {
            var comment = await _db.PostComments.FindAsync(dto.Id);
            comment!.Description = dto.Description;
            comment.Updated = DateTime.Now;
            _db.PostComments.Update(comment);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckIfCommentBelongsToUserAsync(int userId, int commentId, CancellationToken cancellationToken)
        {
            return await _db.PostComments
                .IgnoreAutoIncludes()
                .Include(c => c.User)
                .AnyAsync(c => c.Id == commentId && c.User.Id == userId, cancellationToken);
        }

        public async Task<bool> CheckIfCommentExistsAsync(int commentId, CancellationToken cancellationToken)
        {
            return await _db.PostComments
                .IgnoreAutoIncludes()
                .AnyAsync(c => c.Id == commentId, cancellationToken);
        }

        public async Task<List<GetPostCommentDto>> GetCommentsByPostIdAsync(int postId, CancellationToken cancellationToken)
        {
            return await _db.PostComments
                .IgnoreAutoIncludes()
                .Include(c => c.User)
                .Where(c => c.Post.Id == postId)
                .Select(c => new GetPostCommentDto
                {
                    Id = c.Id,
                    Description = c.Description,
                    UserId = c.User.Id,
                    Created = c.Created,
                    Updated = c.Updated
                })
                .ToListAsync(cancellationToken);
        }
    }
}
