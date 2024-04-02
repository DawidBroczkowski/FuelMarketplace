using FuelMarketplace.Domain.Models;
using FuelMarketplace.Shared.Dtos;
using FuelMarketplace.Shared.Dtos.OfferCommentDtos;
using FuelMarketplace.Shared.Dtos.OfferDtos;

namespace FuelMarketplace.Infrastructure.DataAccess.Interfaces
{
    public interface IOfferRepository
    {
        Task AddOfferAsync(CreateOfferDto offer, int userId, CancellationToken cancellationToken);
        Task<bool> CheckIfOfferBelongsToSalesPointAsync(int offerId, int salesPointId, CancellationToken cancellationToken);
        Task<bool> CheckIfOfferBelongsToUserAsync(int offerId, int userId, CancellationToken cancellationToken);
        Task<bool> CheckIfOfferBelongsToUserAsync(int offerId, string userEmail, CancellationToken cancellationToken);
        Task<bool> CheckIfOfferExistsAsync(int offerId, CancellationToken cancellationToken);
        Task DeleteOfferAsync(Offer offer, CancellationToken cancellationToken);
        Task DeleteOfferByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<GetOfferDto>> GetAllOffersAsync(CancellationToken cancellationToken);
        Task<GetOfferDto?> GetOfferByIdAsync(int id, CancellationToken cancellationToken);
        Task<UserDto?> GetOfferOwnerAsync(int offerId, CancellationToken cancellationToken);
        Task<List<GetOfferDto>> GetOffersByFuelTypeAsync(FuelType type, CancellationToken cancellationToken);
        Task<List<GetOfferDto>> GetOffersBySalesPointIdAsync(int salesPointId, CancellationToken cancellationToken);
        Task<List<GetOfferDto>> GetOffersByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task UpdateOfferAsync(EditOfferDto offer, CancellationToken cancellationToken);
        Task AddImageAsync(int offerId, Guid imageGuid, CancellationToken cancellationToken);
        Task DeleteImageAsync(int offerId, Guid imageGuid, CancellationToken cancellationToken);
        Task AddCommentAsync(int userId, CreateOfferCommentDto dto, CancellationToken cancellationToken);
        Task DeleteCommentAsync(int commentId, CancellationToken cancellationToken);
        Task UpdateCommentAsync(EditOfferCommentDto dto, CancellationToken cancellationToken);
        Task<bool> CheckIfCommentBelongsToUserAsync(int userId, int commentId, CancellationToken cancellationToken);
        Task<bool> CheckIfCommentExistsAsync(int commentId, CancellationToken cancellationToken);
        Task<List<GetOfferCommentDto>> GetCommentsByOfferIdAsync(int offerId, CancellationToken cancellationToken);
    }
}