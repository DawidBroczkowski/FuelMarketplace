using FakeItEasy;
using FluentAssertions;
using FuelMarketplace.Application.Interfaces;
using FuelMarketplace.Application.Services;
using FuelMarketplace.Domain.Models;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using FuelMarketplace.Shared.Dtos;
using FuelMarketplace.Shared.Dtos.OfferCommentDtos;
using FuelMarketplace.Shared.Dtos.OfferDtos;
using FuelMarketplace.Shared.Dtos.SalesPointDtos;
using FuelMarketplace.Shared.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace FuelMarketplace.Tests
{
    public class SalesPointServiceTests
    {
        private readonly ISalesPointRepository _salesPointRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISalesPointService _salesPointService;
        private readonly IServiceProvider _serviceProvider;
        public SalesPointServiceTests()
        {
            _salesPointRepository = A.Fake<ISalesPointRepository>();
            _userRepository = A.Fake<IUserRepository>();
            _serviceProvider = A.Fake<IServiceProvider>();

            _salesPointService = new SalesPointService(_salesPointRepository, _userRepository);
        }

        [Fact]
        public async Task AddSalesPointAsync_ValidDto_AddsSalesPoint()
        {
            var salesPoint = new CreateSalesPointDto
            {
                Name = "Test",
                Description = "Test"
            };

            await _salesPointService.AddSalesPointAsync(salesPoint, 1, default);

            A.CallTo(() => _salesPointRepository.AddSalesPointAsync(salesPoint, 1, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetSalesPointByIdAsync_SalesPointExists_ReturnsSalesPoint()
        {
            var id = 1;
            var salesPoint = new GetSalesPointDto
            {
                Name = "Test",
                Description = "Test"
            };

            A.CallTo(() => _salesPointRepository.GetSalesPointByIdAsync(id, A<CancellationToken>._)).Returns(salesPoint);

            var result = await _salesPointService.GetSalesPointByIdAsync(id, default);

            result.Should().BeEquivalentTo(salesPoint);
        }

        [Fact]
        public async Task GetSalesPointByIdAsync_SalesPointDoesNotExist_ThrowsKeyNotFoundException()
        {
            var id = 1;

            A.CallTo(() => _salesPointRepository.GetSalesPointByIdAsync(id, A<CancellationToken>._)).Returns(null as GetSalesPointDto);

            Func<Task> act = async () => await _salesPointService.GetSalesPointByIdAsync(id, default);

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Can't find sales point.");
        }

        [Fact]
        public async Task GetAllSalesPointsAsync_ReturnsSalesPoints()
        {
            var salesPoints = new List<GetSalesPointDto>
            {
                new GetSalesPointDto
                {
                    Name = "Test",
                    Description = "Test"
                }
            };

            A.CallTo(() => _salesPointRepository.GetAllSalesPointsAsync(A<CancellationToken>._)).Returns(salesPoints);

            var result = await _salesPointService.GetAllSalesPointsAsync(default);

            result.Should().BeEquivalentTo(salesPoints);
        }

        [Fact]
        public async Task DeleteSalesPointAsync_SalesPointExists_DeletesSalesPoint()
        {
            var userId = 1;
            var salesPointId = 1;

            A.CallTo(() => _salesPointRepository.CheckIfSalesPointExistsAsync(salesPointId, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _salesPointRepository.CheckIfSalesPointBelongsToUserAsync(salesPointId, userId, A<CancellationToken>._)).Returns(true);

            await _salesPointService.DeleteSalesPointAsync(userId, salesPointId, default);

            A.CallTo(() => _salesPointRepository.DeleteSalesPointByIdAsync(salesPointId, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteSalesPointAsync_SalesPointDoesNotExist_ThrowsKeyNotFoundException()
        {
            var userId = 1;
            var salesPointId = 1;

            A.CallTo(() => _salesPointRepository.CheckIfSalesPointExistsAsync(salesPointId, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _salesPointService.DeleteSalesPointAsync(userId, salesPointId, default);

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Can't find sales point.");
        }

        [Fact]
        public async Task DeleteSalesPointAsync_UserIsNotOwner_ThrowsAuthorizationException()
        {
            var userId = 1;
            var salesPointId = 1;

            A.CallTo(() => _salesPointRepository.CheckIfSalesPointExistsAsync(salesPointId, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _salesPointRepository.CheckIfSalesPointBelongsToUserAsync(salesPointId, userId, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _salesPointService.DeleteSalesPointAsync(userId, salesPointId, default);

            await act.Should().ThrowAsync<AuthorizationException>().WithMessage("User is not owner.");
        }

        [Fact]
        public async Task EditSalesPointAsync_SalesPointExists_EditsSalesPoint()
        {
            var userId = 1;
            var dto = new EditSalesPointDto
            {
                Id = 1,
                Name = "Test",
                Description = "Test"
            };

            A.CallTo(() => _salesPointRepository.CheckIfSalesPointExistsAsync(dto.Id, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _salesPointRepository.CheckIfSalesPointBelongsToUserAsync(userId, dto.Id, A<CancellationToken>._)).Returns(true);

            await _salesPointService.EditSalesPointAsync(userId, dto, default);

            A.CallTo(() => _salesPointRepository.UpdateSalesPointAsync(dto, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task EditSalesPointAsync_SalesPointDoesNotExist_ThrowsKeyNotFoundException()
        {
            var userId = 1;
            var dto = new EditSalesPointDto
            {
                Id = 1,
                Name = "Test",
                Description = "Test"
            };

            A.CallTo(() => _salesPointRepository.CheckIfSalesPointExistsAsync(dto.Id, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _salesPointService.EditSalesPointAsync(userId, dto, default);

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Can't find sales point.");
        }

        [Fact]
        public async Task EditSalesPointAsync_UserIsNotOwner_ThrowsAuthorizationException()
        {
            var userId = 1;
            var dto = new EditSalesPointDto
            {
                Id = 1,
                Name = "Test",
                Description = "Test"
            };

            A.CallTo(() => _salesPointRepository.CheckIfSalesPointExistsAsync(dto.Id, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _salesPointRepository.CheckIfSalesPointBelongsToUserAsync(userId, dto.Id, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _salesPointService.EditSalesPointAsync(userId, dto, default);

            await act.Should().ThrowAsync<AuthorizationException>().WithMessage("User is not owner.");
        }

        [Fact]
        public async Task EditSalesPointAsync_ValidDto_EditsSalesPoint()
        {
            var userId = 1;
            var dto = new EditSalesPointDto
            {
                Id = 1,
                Name = "Test",
                Description = "Test"
            };

            A.CallTo(() => _salesPointRepository.CheckIfSalesPointExistsAsync(dto.Id, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _salesPointRepository.CheckIfSalesPointBelongsToUserAsync(userId, dto.Id, A<CancellationToken>._)).Returns(true);

            await _salesPointService.EditSalesPointAsync(userId, dto, default);

            A.CallTo(() => _salesPointRepository.UpdateSalesPointAsync(dto, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetSalesPointsByUserIdAsync_UserExists_ReturnsSalesPoints()
        {
            var userId = 1;
            var salesPoints = new List<GetSalesPointDto>
            {
                new GetSalesPointDto
                {
                    Name = "Test",
                    Description = "Test"
                }
            };

            A.CallTo(() => _userRepository.CheckIfUserExistsAsync(userId, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _salesPointRepository.GetSalesPointsByUserIdAsync(userId, A<CancellationToken>._)).Returns(salesPoints);

            var result = await _salesPointService.GetSalesPointsByUserIdAsync(userId, default);

            result.Should().BeEquivalentTo(salesPoints);
        }

        [Fact]
        public async Task GetSalesPointsByUserIdAsync_UserDoesNotExist_ThrowsKeyNotFoundException()
        {
            var userId = 1;

            A.CallTo(() => _userRepository.CheckIfUserExistsAsync(userId, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _salesPointService.GetSalesPointsByUserIdAsync(userId, default);

            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Can't find user.");
        }

        [Fact]
        public async Task GetSalesPointsByUserIdAsync_SalesPointsDoNotExist_ReturnsEmptyList()
        {
            var userId = 1;
            var salesPoints = new List<GetSalesPointDto>{};

            A.CallTo(() => _userRepository.CheckIfUserExistsAsync(userId, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _salesPointRepository.GetSalesPointsByUserIdAsync(userId, A<CancellationToken>._)).Returns(salesPoints);

            var result = await _salesPointService.GetSalesPointsByUserIdAsync(userId, default);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
