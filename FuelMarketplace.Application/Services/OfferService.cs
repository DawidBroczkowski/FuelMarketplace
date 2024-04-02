using FuelMarketplace.Application.Interfaces;
using FuelMarketplace.Domain.Models;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using FuelMarketplace.Shared.Dtos;
using FuelMarketplace.Shared.Dtos.OfferCommentDtos;
using FuelMarketplace.Shared.Dtos.OfferDtos;
using FuelMarketplace.Shared.Exceptions;

namespace FuelMarketplace.Application.Services
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISalesPointRepository _salesPointRepository;

        public OfferService(IOfferRepository repository, ISalesPointRepository salesPointRepository, IUserRepository userRepository)
        {
            _offerRepository = repository;
            _salesPointRepository = salesPointRepository;
            _userRepository = userRepository;
        }

        public async Task<List<GetOfferDto>> GetAllOffersAsync(CancellationToken cancellationToken)
        {
            return await _offerRepository.GetAllOffersAsync(cancellationToken);
        }

        public async Task<GetOfferDto?> GetOfferByIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _offerRepository.GetOfferByIdAsync(id, cancellationToken);
            if (result is null)
            {
                var ex = new KeyNotFoundException("Can't find offer.");
                ex.Data.Add("Id", "Offer not found.");
                throw ex;
            }
            return result;
        }

        public async Task<List<GetOfferDto>> GetOfferBySalesPointIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _offerRepository.GetOffersBySalesPointIdAsync(id, cancellationToken);
            if (result is null)
            {
                var ex = new KeyNotFoundException("Can't find sales point.");
                ex.Data.Add("Id", "Sales point not found.");
                throw ex;
            }
            return result;
        }

        public async Task AddOfferAsync(CreateOfferDto dto, int userId, CancellationToken cancellationToken)
        {
            await _offerRepository.AddOfferAsync(dto, userId, cancellationToken);
        }

        public async Task DeleteOfferAsync(int userId, int offerId, CancellationToken cancellationToken, bool moderator = false)
        {
            var offerExists = await _offerRepository.CheckIfOfferExistsAsync(offerId, cancellationToken);
            if (offerExists is false)
            {
                var ex = new KeyNotFoundException("Can't find offer.");
                ex.Data.Add("Id", "Offer not found.");
                throw ex;
            }
            if (await _offerRepository.CheckIfOfferBelongsToUserAsync(offerId, userId, cancellationToken) is false
                && moderator is false)
            {
                var ex = new AuthorizationException("User is not owner.");
                ex.Data.Add("User", "User is not the owner of this post.");
                throw ex;
            }
            await _offerRepository.DeleteOfferByIdAsync(offerId, cancellationToken);
        }

        public async Task EditOfferAsync(int userId, EditOfferDto dto, CancellationToken cancellationToken)
        {
            var offerExists = await _offerRepository.CheckIfOfferExistsAsync(dto.Id, cancellationToken);
            if (offerExists is false)
            {
                var ex = new KeyNotFoundException("Can't find offer.");
                ex.Data.Add("Id", "Offer not found.");
                throw ex;
            }
            if (await _offerRepository.CheckIfOfferBelongsToUserAsync(dto.Id, userId, cancellationToken) is false)
            {
                var ex = new AuthorizationException("User is not owner.");
                ex.Data.Add("User", "User is not the owner of this post.");
                throw ex;
            }
            if (dto.SalesPointId is not null
                && await _salesPointRepository.CheckIfSalesPointBelongsToUserAsync((int)dto.SalesPointId, userId, cancellationToken) is false)
            {
                var ex = new AuthorizationException("Sales point does not belong to user.");
                ex.Data.Add("SalesPoint", "This user is not the owner of this sales point.");
                throw ex;
            }
            await _offerRepository.UpdateOfferAsync(dto, cancellationToken);
        }

        public async Task<List<GetOfferDto>> GetOffersByFuelTypeAsync(FuelType fuelType, CancellationToken cancellationToken)
        {
            return await _offerRepository.GetOffersByFuelTypeAsync(fuelType, cancellationToken);
        }

        public async Task<List<GetOfferDto>> GetOffersByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var userExists = await _userRepository.CheckIfUserExistsAsync(userId, cancellationToken);
            if (userExists is false)
            {
                var ex = new KeyNotFoundException("User not found.");
                ex.Data.Add("Id", "User with that Id does not exist.");
                throw ex;
            }
            return await _offerRepository.GetOffersByUserIdAsync(userId, cancellationToken);
        }

        public async Task<UserDto> GetOfferOwnerAsync(int id, CancellationToken cancellationToken)
        {
            var offer = await _offerRepository.GetOfferByIdAsync(id, cancellationToken);
            if (offer is null)
            {
                var ex = new KeyNotFoundException("Can't find offer.");
                ex.Data.Add("Id", "Offer not found.");
                throw ex;
            }
            return (await _userRepository.GetUserAsync(offer.UserId, cancellationToken))!;
        }

        public async Task AddCommentAsync(int userId, CreateOfferCommentDto dto, CancellationToken cancellationToken)
        {
            var offerExists = await _offerRepository.CheckIfOfferExistsAsync(dto.OfferId, cancellationToken);
            if (offerExists is false)
            {
                var ex = new KeyNotFoundException("Can't find offer.");
                ex.Data.Add("Id", "Offer not found.");
                throw ex;
            }
            await _offerRepository.AddCommentAsync(userId, dto, cancellationToken);
        }

        public async Task DeleteCommentAsync(int userId, int commentId, CancellationToken cancellationToken, bool moderator = false)
        {
            var commentExists = await _offerRepository.CheckIfCommentExistsAsync(commentId, cancellationToken);
            if (commentExists is false)
            {
                var ex = new KeyNotFoundException("Can't find comment.");
                ex.Data.Add("Id", "Comment not found.");
                throw ex;
            }
            if (await _offerRepository.CheckIfCommentBelongsToUserAsync(userId, commentId, cancellationToken) is false
                && moderator is false)
            {
                var ex = new AuthorizationException("User is not owner.");
                ex.Data.Add("User", "User is not the owner of this comment.");
                throw ex;
            }
            await _offerRepository.DeleteCommentAsync(commentId, cancellationToken);
        }

        public async Task EditCommentAsync(int userId, EditOfferCommentDto dto, CancellationToken cancellationToken)
        {
            var commentExists = await _offerRepository.CheckIfCommentExistsAsync(dto.Id, cancellationToken);
            if (commentExists is false)
            {
                var ex = new KeyNotFoundException("Can't find comment.");
                ex.Data.Add("Id", "Comment not found.");
                throw ex;
            }
            if (await _offerRepository.CheckIfCommentBelongsToUserAsync(userId, dto.Id, cancellationToken) is false)
            {
                var ex = new AuthorizationException("User is not owner.");
                ex.Data.Add("User", "User is not the owner of this comment.");
                throw ex;
            }
            await _offerRepository.UpdateCommentAsync(dto, cancellationToken);
        }

        public async Task<List<GetOfferCommentDto>> GetCommentsByOfferIdAsync(int id, CancellationToken cancellationToken)
        {
            var offerExists = await _offerRepository.CheckIfOfferExistsAsync(id, cancellationToken);
            if (offerExists is false)
            {
                var ex = new KeyNotFoundException("Can't find offer.");
                ex.Data.Add("Id", "Offer not found.");
                throw ex;
            }
            return await _offerRepository.GetCommentsByOfferIdAsync(id, cancellationToken);
        }
    }
}
