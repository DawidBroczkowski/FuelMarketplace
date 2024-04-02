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
        private readonly IPostService _postService;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public PostServiceTests()
        {
            _postRepository = A.Fake<IPostRepository>();
            _userRepository = A.Fake<IUserRepository>();
            _postService = new PostService(_postRepository, _userRepository);
        }

        [Fact]
        public async Task GetAllPostsAsync_PostsExist_ReturnsPosts()
        {
            A.CallTo(() => _postRepository.GetAllPostsAsync(default))
                .Returns(new List<GetPostDto>
                {
                    new GetPostDto { Id = 1, Title = "Test1" },
                    new GetPostDto { Id = 2, Title = "Test2" }
                });

            var result = await _postService.GetAllPostsAsync(default);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().ContainSingle(x => x.Id == 1);
            result.Should().ContainSingle(x => x.Id == 2);
            result.Should().ContainEquivalentOf(new GetPostDto { Id = 1, Title = "Test1" });
            result.Should().ContainEquivalentOf(new GetPostDto { Id = 2, Title = "Test2" });
        }

        [Fact]
        public async Task GetPostByIdAsync_PostExists_ReturnsPost()
        {
            A.CallTo(() => _postRepository.GetPostByIdAsync(1, default)).Returns(new GetPostDto { Id = 1, Title = "Test" });

            var result = await _postService.GetPostByIdAsync(1, default);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new GetPostDto { Id = 1, Title = "Test" });
        }

        [Fact]
        public async Task GetPostByIdAsync_PostDoesNotExist_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _postRepository.GetPostByIdAsync(1, A<CancellationToken>._)).Returns(null as GetPostDto);

            Func<Task> act = async () => await _postService.GetPostByIdAsync(1, default);

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Can't find post.");
        }

        [Fact]
        public async Task AddPostAsync_ValidPost_AddsPost()
        {
            var dto = new CreatePostDto { Title = "Test" };

            await _postService.AddPostAsync(dto, 1, default);

            A.CallTo(() => _postRepository.AddPostAsync(dto, 1, A<CancellationToken>._)).MustHaveHappened();
        }

        [Fact]
        public async Task DeletePostAsync_PostExists_DeletesPost()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _postRepository.CheckIfPostBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(true);

            await _postService.DeletePostAsync(1, 1, default);

            A.CallTo(() => _postRepository.DeletePostByIdAsync(1, A<CancellationToken>._)).MustHaveHappened();
        }

        [Fact]
        public async Task DeletePostAsync_PostDoesNotExist_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _postService.DeletePostAsync(1, 1, default);

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Can't find post.");
        }

        [Fact]
        public async Task DeletePostAsync_UserIsNotOwner_ThrowsAuthorizationException()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, A<CancellationToken>._)).Returns(true);

            A.CallTo(() => _postRepository.CheckIfPostBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _postService.DeletePostAsync(1, 1, default);

            await act.Should().ThrowAsync<AuthorizationException>().WithMessage("User is not owner.");
        }

        [Fact]
        public async Task EditPostAsync_PostExists_EditsPost()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _postRepository.CheckIfPostBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(true);

            var dto = new EditPostDto { Id = 1, Title = "Test" };

            await _postService.EditPostAsync(1, dto, default);

            A.CallTo(() => _postRepository.UpdatePostAsync(dto, A<CancellationToken>._)).MustHaveHappened();
        }

        [Fact]
        public async Task EditPostAsync_PostDoesNotExist_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _postService.EditPostAsync(1, new EditPostDto { Id = 1 }, default);

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Can't find post.");
        }

        [Fact]
        public async Task EditPostAsync_UserIsNotOwner_ThrowsAuthorizationException()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _postRepository.CheckIfPostBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _postService.EditPostAsync(1, new EditPostDto { Id = 1 }, default);

            await act.Should().ThrowAsync<AuthorizationException>().WithMessage("User is not owner.");
        }

        [Fact]
        public async Task GetPostsByFuelTypeAsync_PostsExist_ReturnsPosts()
        {
            A.CallTo(() => _postRepository.GetPostsByFuelTypeAsync(FuelType.peacoal, A<CancellationToken>._))
                .Returns(new List<GetPostDto>
                {
                    new GetPostDto { Id = 1, Title = "Test1", FuelType = FuelType.peacoal },
                    new GetPostDto { Id = 2, Title = "Test2", FuelType = FuelType.peacoal }
                });

            var result = await _postService.GetPostsByFuelTypeAsync(FuelType.peacoal, default);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().ContainSingle(x => x.Id == 1);
            result.Should().ContainSingle(x => x.Id == 2);
            result.Should().ContainEquivalentOf(new GetPostDto { Id = 1, Title = "Test1", FuelType = FuelType.peacoal });
            result.Should().ContainEquivalentOf(new GetPostDto { Id = 2, Title = "Test2", FuelType = FuelType.peacoal });
        }

        [Fact]
        public async Task GetPostsByFuelTypeAsync_PostsDoNotExist_ReturnsEmptyList()
        {
            A.CallTo(() => _postRepository.GetPostsByFuelTypeAsync(FuelType.peacoal, A<CancellationToken>._)).Returns(new List<GetPostDto>());

            var result = await _postService.GetPostsByFuelTypeAsync(FuelType.peacoal, default);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task AddCommentAsync_ValidComment_AddsComment()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, A<CancellationToken>._))
                .Returns(true);

            var dto = new CreatePostCommentDto { PostId = 1, Description = "Test" };

            await _postService.AddCommentAsync(1, dto, default);

            A.CallTo(() => _postRepository.AddCommentAsync(1, dto, A<CancellationToken>._))
                .MustHaveHappened();
        }

        [Fact]
        public async Task AddCommentAsync_PostDoesNotExist_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _postRepository.CheckIfPostExistsAsync(1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _postService.AddCommentAsync(1, new CreatePostCommentDto(), default);

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Can't find post.");
        }

        [Fact]
        public async Task DeleteCommentAsync_CommentExists_DeletesComment()
        {
            A.CallTo(() => _postRepository.CheckIfCommentExistsAsync(1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _postRepository.CheckIfCommentBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(true);

            await _postService.DeleteCommentAsync(1, 1, default);

            A.CallTo(() => _postRepository.DeleteCommentAsync(1, A<CancellationToken>._)).MustHaveHappened();
        }

        [Fact]
        public async Task DeleteCommentAsync_CommentDoesNotExist_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _postRepository.CheckIfCommentExistsAsync(1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _postService.DeleteCommentAsync(1, 1, default);

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Can't find comment.");
        }

        [Fact]
        public async Task DeleteCommentAsync_UserIsNotOwner_ThrowsAuthorizationException()
        {
            A.CallTo(() => _postRepository.CheckIfCommentExistsAsync(1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _postRepository.CheckIfCommentBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _postService.DeleteCommentAsync(1, 1, default);

            await act.Should().ThrowAsync<AuthorizationException>().WithMessage("User is not owner.");
        }

        [Fact]
        public async Task EditCommentAsync_CommentExists_EditsComment()
        {
            A.CallTo(() => _postRepository.CheckIfCommentExistsAsync(1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _postRepository.CheckIfCommentBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(true);

            var dto = new EditPostCommentDto { Id = 1, Description = "Test" };

            await _postService.EditCommentAsync(1, dto, default);

            A.CallTo(() => _postRepository.UpdateCommentAsync(dto, A<CancellationToken>._)).MustHaveHappened();
        }

        [Fact]
        public async Task EditCommentAsync_CommentDoesNotExist_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _postRepository.CheckIfCommentExistsAsync(1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _postService.EditCommentAsync(1, new EditPostCommentDto()
            {
                Id = 1,
                Description = "Test"
            }, default);

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Can't find comment.");
        }

        [Fact]
        public async Task EditCommentAsync_UserIsNotOwner_ThrowsAuthorizationException()
        {
            A.CallTo(() => _postRepository.CheckIfCommentExistsAsync(1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _postRepository.CheckIfCommentBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _postService.EditCommentAsync(1, new EditPostCommentDto()
            {
                Id = 1,
                Description = "Test"
            }, default);

            await act.Should().ThrowAsync<AuthorizationException>().WithMessage("User is not owner.");
        }
    }
}
