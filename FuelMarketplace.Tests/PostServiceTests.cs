using FakeItEasy;
using FluentAssertions;
using FuelMarketplace.Application.Interfaces;
using FuelMarketplace.Application.Services;
using FuelMarketplace.Domain.Models;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using FuelMarketplace.Shared.Dtos;
using FuelMarketplace.Shared.Dtos.PostCommentDtos;
using FuelMarketplace.Shared.Dtos.PostDtos;
using FuelMarketplace.Shared.Exceptions;
namespace FuelMarketplace.Tests
{
    public class PostServiceTests
    {
        private IPostRepository _postRepository;
        private IUserRepository _userRepository;
        private PostService _postService;

        public PostServiceTests()
        {
            _postRepository = A.Fake<IPostRepository>();
            _userRepository = A.Fake<IUserRepository>();
            _postService = new PostService(_postRepository, _userRepository);
        }

        [Fact]
        public void PostService_GetPostsByUserIdAsync_ReturnsPost()
        {
            A.CallTo(() => _userRepository.CheckIfUserExistsAsync(1, default))
                .Returns(true);

            A.CallTo(() => _postRepository
            .GetPostsByUserIdAsync(1, default))
                .Returns(new List<GetPostDto>
                {
                        new GetPostDto { Id = 1, Title = "Test", Description = "Test", UserId = 1 },
                        new GetPostDto { Id = 5, Title = "Test2", Description = "Test2", UserId = 1 }
                });

            var result = _postService.GetPostsByUserIdAsync(1, default).Result;

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().ContainSingle(x => x.Id == 1);
            result.Should().ContainSingle(x => x.Id == 5);
            result.Should().ContainEquivalentOf(new GetPostDto { Id = 1, Title = "Test", Description = "Test", UserId = 1 });
            result.Should().ContainEquivalentOf(new GetPostDto { Id = 5, Title = "Test2", Description = "Test2", UserId = 1 });
        }

        [Fact]
        public void PostService_GetPostsByUserIdAsync_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _userRepository.CheckIfUserExistsAsync(1, default))
                .Returns(false);

            Func<Task> act = async () => await _postService.GetPostsByUserIdAsync(1, default);

            act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Can't find user.");
        }

        [Fact]
        public void PostService_GetPostByIdAsync_ReturnsPost()
        {
            A.CallTo(() => _postRepository.GetPostByIdAsync(1, default))
                .Returns(new GetPostDto { Id = 1, Title = "Test", Description = "Test", UserId = 1 });

            var result = _postService.GetPostByIdAsync(1, default).Result;

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new GetPostDto { Id = 1, Title = "Test", Description = "Test", UserId = 1 });
        }

        [Fact]
        public void PostService_GetPostByIdAsync_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _postRepository.GetPostByIdAsync(1, default))
                .Returns(null as GetPostDto);

            Func<Task> act = async () => await _postService.GetPostByIdAsync(1, default);

            act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Can't find post.");
        }

        [Fact]
        public void PostService_DeletePostAsync_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, default))
                .Returns(false);

            Func<Task> act = async () => await _postService.DeletePostAsync(1, 1, default);

            act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Can't find post.");
        }

        [Fact]
        public void PostService_DeletePostAsync_ThrowsAuthorizationException()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, default))
                .Returns(true);

            A.CallTo(() => _postRepository.CheckIfPostBelongsToUserAsync(1, 1, default))
                .Returns(false);

            Func<Task> act = async () => await _postService.DeletePostAsync(1, 1, default);

            act.Should().ThrowAsync<AuthorizationException>()
                .WithMessage("User is not owner.");
        }

        [Fact]
        public async void PostService_DeletePostAsync_DeletesPost()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, default))
                .Returns(true);

            A.CallTo(() => _postRepository.CheckIfPostBelongsToUserAsync(1, 1, default))
                .Returns(true);

            await _postService.DeletePostAsync(1, 1, default);

            A.CallTo(() => _postRepository.DeletePostByIdAsync(1, default))
                .MustHaveHappened();
        }

        [Fact]
        public void PostService_GetPostsByFuelTypeAsync_ReturnsPost()
        {
            A.CallTo(() => _postRepository.GetPostsByFuelTypeAsync(FuelType.coal, default))
                .Returns(new List<GetPostDto>
                {
                        new GetPostDto { Id = 1, Title = "Test", Description = "Test", UserId = 1, FuelType = FuelType.coal },
                        new GetPostDto { Id = 5, Title = "Test2", Description = "Test2", UserId = 1, FuelType = FuelType.coal }
                });

            var result = _postService.GetPostsByFuelTypeAsync(FuelType.coal, default).Result;

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().ContainSingle(x => x.Id == 1);
            result.Should().ContainSingle(x => x.Id == 5);
            result.Should().ContainEquivalentOf(new GetPostDto { Id = 1, Title = "Test", Description = "Test", UserId = 1, FuelType = FuelType.coal });
            result.Should().ContainEquivalentOf(new GetPostDto { Id = 5, Title = "Test2", Description = "Test2", UserId = 1, FuelType = FuelType.coal });
            result.Should().OnlyContain(x => x.FuelType == FuelType.coal);
        }

        [Fact]
        public void PostService_GetPostsByFuelTypeAsync_ReturnsEmptyList()
        {
            A.CallTo(() => _postRepository.GetPostsByFuelTypeAsync(FuelType.coal, default))
                .Returns(new List<GetPostDto>());

            var result = _postService.GetPostsByFuelTypeAsync(FuelType.coal, default).Result;

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void PostService_EditPostAsync_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, default))
                .Returns(false);

            Func<Task> act = async () => await _postService.EditPostAsync(1, new EditPostDto { Id = 1 }, default);

            act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Can't find post.");
        }

        [Fact]
        public void PostService_EditPostAsync_ThrowsAuthorizationException()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, default))
                .Returns(true);

            A.CallTo(() => _postRepository.CheckIfPostBelongsToUserAsync(1, 1, default))
                .Returns(false);

            Func<Task> act = async () => await _postService.EditPostAsync(1, new EditPostDto { Id = 1 }, default);

            act.Should().ThrowAsync<AuthorizationException>()
                .WithMessage("User is not owner.");
        }

        [Fact]
        public async void PostService_EditPostAsync_UpdatesPost()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, default))
                .Returns(true);

            A.CallTo(() => _postRepository.CheckIfPostBelongsToUserAsync(1, 1, default))
                .Returns(true);

            var dto = new EditPostDto { Id = 1, Title = "Test", Description = "Test", FuelType = FuelType.coal };

            await _postService.EditPostAsync(1, dto, default);

            A.CallTo(() => _postRepository.UpdatePostAsync(dto, default))
                .MustHaveHappened();
        }

        [Fact]
        public void PostService_AddCommentAsync_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, default))
                .Returns(false);

            Func<Task> act = async () => await _postService.AddCommentAsync(1, new CreatePostCommentDto { PostId = 1 }, default);

            act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Can't find post.");
        }

        [Fact]
        public async void PostService_AddCommentAsync_AddsComment()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, default))
                .Returns(true);

            var dto = new CreatePostCommentDto { PostId = 1, Description = "Test" };

            await _postService.AddCommentAsync(1, dto, default);

            A.CallTo(() => _postRepository.AddCommentAsync(1, dto, default))
                .MustHaveHappened();
        }

        [Fact]
        public void PostService_GetPostOwnerAsync_ReturnsUser()
        {
            A.CallTo(() => _postRepository.GetPostOwnerAsync(1, default))
                .Returns(new UserDto { Id = 1, Name = "Test" });

            var result = _postService.GetPostOwnerAsync(1, default).Result;

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new UserDto { Id = 1, Name = "Test" });
        }

        [Fact]
        public void PostService_GetPostOwnerAsync_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _postRepository.GetPostOwnerAsync(1, default))
                .Returns(null as UserDto);

            Func<Task> act = async () => await _postService.GetPostOwnerAsync(1, default);

            act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Can't find post.");
        }

        [Fact]
        public void PostService_GetCommentsByPostIdAsync_ReturnsComments()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, default))
                .Returns(true);

            A.CallTo(() => _postRepository.GetCommentsByPostIdAsync(1, default))
                .Returns(new List<GetPostCommentDto>
                {
                    new GetPostCommentDto { Id = 1, Description = "Test", UserId = 1 },
                    new GetPostCommentDto { Id = 2, Description = "Test2", UserId = 1 }
                });

            var result = _postService.GetCommentsByPostIdAsync(1, default).Result;

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().ContainSingle(x => x.Id == 1);
            result.Should().ContainSingle(x => x.Id == 2);
            result.Should().ContainEquivalentOf(new GetPostCommentDto { Id = 1, Description = "Test", UserId = 1 });
            result.Should().ContainEquivalentOf(new GetPostCommentDto { Id = 2, Description = "Test2", UserId = 1 });
        }

        [Fact]
        public void PostService_GetCommentsByPostIdAsync_ReturnsEmptyList()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, default))
                .Returns(true);

            A.CallTo(() => _postRepository.GetCommentsByPostIdAsync(1, default))
                .Returns(new List<GetPostCommentDto>());

            var result = _postService.GetCommentsByPostIdAsync(1, default).Result;

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void PostService_DeleteCommentAsync_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _postRepository.CheckIfCommentExistsAsync(1, default))
                .Returns(false);

            Func<Task> act = async () => await _postService.DeleteCommentAsync(1, 1, default);

            act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Can't find comment.");
        }

        [Fact]
        public void PostService_DeleteCommentAsync_ThrowsAuthorizationException()
        {
            A.CallTo(() => _postRepository.CheckIfCommentExistsAsync(1, default))
                .Returns(true);

            A.CallTo(() => _postRepository.CheckIfCommentBelongsToUserAsync(1, 1, default))
                .Returns(false);

            Func<Task> act = async () => await _postService.DeleteCommentAsync(1, 1, default);

            act.Should().ThrowAsync<AuthorizationException>()
                .WithMessage("User is not owner.");
        }

        [Fact]
        public async void PostService_DeleteCommentAsync_DeletesComment()
        {
            A.CallTo(() => _postRepository.CheckIfCommentExistsAsync(1, default))
                .Returns(true);

            A.CallTo(() => _postRepository.CheckIfCommentBelongsToUserAsync(1, 1, default))
                .Returns(true);

            await _postService.DeleteCommentAsync(1, 1, default);

            A.CallTo(() => _postRepository.DeleteCommentAsync(1, default))
                .MustHaveHappened();
        }

        [Fact]
        public void PostService_EditCommentAsync_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _postRepository.CheckIfCommentExistsAsync(1, default))
                .Returns(false);

            Func<Task> act = async () => await _postService.EditCommentAsync(1, new EditPostCommentDto { Id = 1 }, default);

            act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Can't find comment.");
        }

        [Fact]
        public void PostService_EditCommentAsync_ThrowsAuthorizationException()
        {
            A.CallTo(() => _postRepository.CheckIfCommentExistsAsync(1, default))
                .Returns(true);

            A.CallTo(() => _postRepository.CheckIfCommentBelongsToUserAsync(1, 1, default))
                .Returns(false);

            Func<Task> act = async () => await _postService.EditCommentAsync(1, new EditPostCommentDto { Id = 1 }, default);

            act.Should().ThrowAsync<AuthorizationException>()
                .WithMessage("User is not owner.");
        }

        [Fact]
        public async void PostService_EditCommentAsync_UpdatesComment()
        {
            A.CallTo(() => _postRepository.CheckIfCommentExistsAsync(1, default))
                .Returns(true);

            A.CallTo(() => _postRepository.CheckIfCommentBelongsToUserAsync(1, 1, default))
                .Returns(true);

            var dto = new EditPostCommentDto { Id = 1, Description = "Test" };

            await _postService.EditCommentAsync(1, dto, default);

            A.CallTo(() => _postRepository.UpdateCommentAsync(dto, default))
                .MustHaveHappened();
        }
    }
}
