using FakeItEasy;
using FluentAssertions;
using FuelMarketplace.Application.Interfaces;
using FuelMarketplace.Application.Services;
using FuelMarketplace.Domain.Models;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using FuelMarketplace.Shared.Dtos;
using FuelMarketplace.Shared.Dtos.OfferCommentDtos;
using FuelMarketplace.Shared.Dtos.OfferDtos;
using FuelMarketplace.Shared.Exceptions;

namespace FuelMarketplace.Tests
{
    public class OfferServiceTests
    {
        private readonly IOfferService _offerService;
        private readonly IOfferRepository _offerRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISalesPointRepository _salesPointRepository;

        public OfferServiceTests()
        {
            _offerRepository = A.Fake<IOfferRepository>();
            _userRepository = A.Fake<IUserRepository>();
            _salesPointRepository = A.Fake<ISalesPointRepository>();
            _offerService = new OfferService(_offerRepository, _salesPointRepository, _userRepository);
        }

        [Fact]
        public async Task GetAllOffersAsync_OffersExist_ReturnsOffers()
        {
            var offers = new List<GetOfferDto>
            {
                new GetOfferDto
                {
                    Id = 1,
                    SalesPointId = 1,
                    UserId = 1
                }
            };

            A.CallTo(() => _offerRepository.GetAllOffersAsync(A<CancellationToken>._)).Returns(offers);

            var result = await _offerService.GetAllOffersAsync(default);

            result.Should().BeEquivalentTo(offers);
        }

        [Fact]
        public async Task GetOfferByIdAsync_OfferExists_ReturnsOffer()
        {
            var offer = new GetOfferDto
            {
                Id = 1,
                SalesPointId = 1,
                UserId = 1
            };

            A.CallTo(() => _offerRepository.GetOfferByIdAsync(1, A<CancellationToken>._)).Returns(offer);

            var result = await _offerService.GetOfferByIdAsync(1, default);

            result.Should().BeEquivalentTo(offer);
        }

        [Fact]
        public async Task GetOfferByIdAsync_OfferDoesNotExist_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _offerRepository.GetOfferByIdAsync(1, A<CancellationToken>._)).Returns(null as GetOfferDto);

            Func<Task> act = async () => await _offerService.GetOfferByIdAsync(1, default);
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetOfferBySalesPointIdAsync_OffersExist_ReturnsOffers()
        {
            var offers = new List<GetOfferDto>
            {
                new GetOfferDto
                {
                    Id = 1,
                    SalesPointId = 1,
                    UserId = 1
                }
            };

            A.CallTo(() => _offerRepository.GetOffersBySalesPointIdAsync(1, A<CancellationToken>._)).Returns(offers);

            var result = await _offerService.GetOfferBySalesPointIdAsync(1, default);

            result.Should().BeEquivalentTo(offers);
        }

        [Fact]
        public async Task GetOfferBySalesPointIdAsync_SalesPointDoesNotExist_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _offerRepository.GetOffersBySalesPointIdAsync(1, A<CancellationToken>._))!.Returns(null as List<GetOfferDto>);

            Func<Task> act = async () => await _offerService.GetOfferBySalesPointIdAsync(1, default);
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task AddOfferAsync_ValidOffer_AddsOffer()
        {
            var offer = new CreateOfferDto
            {
                SalesPointId = 1,
                Title = "Title",
                Description = "Description",
                Price = 1.0m,
                FuelType = FuelType.peacoal
            };

            await _offerService.AddOfferAsync(offer, 1, default);

            A.CallTo(() => _offerRepository.AddOfferAsync(offer, 1, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteOfferAsync_OfferExists_DeletesOffer()
        {
            A.CallTo(() => _offerRepository.CheckIfOfferExistsAsync(1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _offerRepository.CheckIfOfferBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(true);

            await _offerService.DeleteOfferAsync(1, 1, default);

            A.CallTo(() => _offerRepository.DeleteOfferByIdAsync(1, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteOfferAsync_OfferDoesNotExist_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _offerRepository.CheckIfOfferExistsAsync(1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _offerService.DeleteOfferAsync(1, 1, default);
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task DeleteOfferAsync_UserIsNotOwner_ThrowsAuthorizationException()
        {
            A.CallTo(() => _offerRepository.CheckIfOfferExistsAsync(1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _offerRepository.CheckIfOfferBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _offerService.DeleteOfferAsync(1, 1, default);
            await act.Should().ThrowAsync<AuthorizationException>();
        }

        [Fact]
        public async Task EditOfferAsync_OfferExists_EditsOffer()
        {
            var offer = new EditOfferDto
            {
                Id = 1,
                SalesPointId = 1,
                Title = "Title",
                Description = "Description",
                Price = 1.0m,
                FuelType = FuelType.peacoal
            };

            A.CallTo(() => _offerRepository.CheckIfOfferExistsAsync(1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _offerRepository.CheckIfOfferBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _salesPointRepository.CheckIfSalesPointBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(true);

            await _offerService.EditOfferAsync(1, offer, default);

            A.CallTo(() => _offerRepository.UpdateOfferAsync(offer, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task EditOfferAsync_OfferDoesNotExist_ThrowsKeyNotFoundException()
        {
            var offer = new EditOfferDto
            {
                Id = 1,
                SalesPointId = 1,
                Title = "Title",
                Description = "Description",
                Price = 1.0m,
                FuelType = FuelType.peacoal
            };

            A.CallTo(() => _offerRepository.CheckIfOfferExistsAsync(1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _offerService.EditOfferAsync(1, offer, default);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
        [Fact]
        public async Task EditOfferAsync_UserIsNotOwner_ThrowsAuthorizationException()
        {
            var offer = new EditOfferDto
            {
                Id = 1,
                SalesPointId = 1,
                Title = "Title",
                Description = "Description",
                Price = 1.0m,
                FuelType = FuelType.peacoal
            };

            A.CallTo(() => _offerRepository.CheckIfOfferExistsAsync(1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _offerRepository.CheckIfOfferBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _offerService.EditOfferAsync(1, offer, default);

            await act.Should().ThrowAsync<AuthorizationException>();
        }

        [Fact]
        public async Task EditOfferAsync_SalesPointDoesNotBelongToUser_ThrowsAuthorizationException()
        {
            var offer = new EditOfferDto
            {
                Id = 1,
                SalesPointId = 1,
                Title = "Title",
                Description = "Description",
                Price = 1.0m,
                FuelType = FuelType.peacoal
            };

            A.CallTo(() => _offerRepository.CheckIfOfferExistsAsync(1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _offerRepository.CheckIfOfferBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _salesPointRepository.CheckIfSalesPointBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _offerService.EditOfferAsync(1, offer, default);
            await act.Should().ThrowAsync<AuthorizationException>();
        }

        [Fact]
        public async Task GetOffersByFuelTypeAsync_OffersExist_ReturnsOffers()
        {
            var offers = new List<GetOfferDto>
            {
                new GetOfferDto
                {
                    Id = 1,
                    SalesPointId = 1,
                    UserId = 1
                }
            };

            A.CallTo(() => _offerRepository.GetOffersByFuelTypeAsync(FuelType.peacoal, A<CancellationToken>._)).Returns(offers);

            var result = await _offerService.GetOffersByFuelTypeAsync(FuelType.peacoal, default);

            result.Should().BeEquivalentTo(offers);
        }

        [Fact]
        public async Task GetOffersByFuelTypeAsync_OffersDoNotExist_ReturnsEmptyList()
        {
            A.CallTo(() => _offerRepository.GetOffersByFuelTypeAsync(FuelType.peacoal, A<CancellationToken>._)).Returns(new List<GetOfferDto>());

            var result = await _offerService.GetOffersByFuelTypeAsync(FuelType.peacoal, default);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task AddCommentAsync_ValidComment_AddsComment()
        {
            var comment = new CreateOfferCommentDto
            {
                OfferId = 1,
                Description = "Content"
            };

            A.CallTo(() => _offerRepository.CheckIfOfferExistsAsync(1, A<CancellationToken>._)).Returns(true);

            await _offerService.AddCommentAsync(1, comment, default);

            A.CallTo(() => _offerRepository.AddCommentAsync(1, comment, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddCommentAsync_OfferDoesNotExist_ThrowsKeyNotFoundException()
        {
            var comment = new CreateOfferCommentDto
            {
                OfferId = 1,
                Description = "Content"
            };

            A.CallTo(() => _offerRepository.CheckIfOfferExistsAsync(1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _offerService.AddCommentAsync(1, comment, default);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task DeleteCommentAsync_CommentExists_DeletesComment()
        {
            A.CallTo(() => _offerRepository.CheckIfCommentExistsAsync(1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _offerRepository.CheckIfCommentBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(true);

            await _offerService.DeleteCommentAsync(1, 1, default);

            A.CallTo(() => _offerRepository.DeleteCommentAsync(1, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteCommentAsync_CommentDoesNotExist_ThrowsKeyNotFoundException()
        {
            A.CallTo(() => _offerRepository.CheckIfCommentExistsAsync(1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _offerService.DeleteCommentAsync(1, 1, default);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task DeleteCommentAsync_UserIsNotOwner_ThrowsAuthorizationException()
        {
            A.CallTo(() => _offerRepository.CheckIfCommentExistsAsync(1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _offerRepository.CheckIfCommentBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _offerService.DeleteCommentAsync(1, 1, default);
            await act.Should().ThrowAsync<AuthorizationException>();
        }

        [Fact]
        public async Task EditCommentAsync_CommentExists_EditsComment()
        {
            var comment = new EditOfferCommentDto
            {
                Id = 1,
                Description = "Content"
            };

            A.CallTo(() => _offerRepository.CheckIfCommentExistsAsync(1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _offerRepository.CheckIfCommentBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(true);

            await _offerService.EditCommentAsync(1, comment, default);

            A.CallTo(() => _offerRepository.UpdateCommentAsync(comment, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task EditCommentAsync_CommentDoesNotExist_ThrowsKeyNotFoundException()
        {
            var comment = new EditOfferCommentDto
            {
                Id = 1,
                Description = "Content"
            };

            A.CallTo(() => _offerRepository.CheckIfCommentExistsAsync(1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _offerService.EditCommentAsync(1, comment, default);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task EditCommentAsync_UserIsNotOwner_ThrowsAuthorizationException()
        {
            var comment = new EditOfferCommentDto
            {
                Id = 1,
                Description = "Content"
            };

            A.CallTo(() => _offerRepository.CheckIfCommentExistsAsync(1, A<CancellationToken>._)).Returns(true);
            A.CallTo(() => _offerRepository.CheckIfCommentBelongsToUserAsync(1, 1, A<CancellationToken>._)).Returns(false);

            Func<Task> act = async () => await _offerService.EditCommentAsync(1, comment, default);
            await act.Should().ThrowAsync<AuthorizationException>();
        }
    }
}
