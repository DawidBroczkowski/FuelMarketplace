using FuelMarketplace.Domain.Models;
using FuelMarketplace.Shared.Dtos;
using FuelMarketplace.Shared.Dtos.OfferCommentDtos;
using FuelMarketplace.Shared.Dtos.OfferDtos;

namespace FuelMarketplace.Application.Interfaces
{
    public interface IOfferService
    {
        Task AddOfferAsync(CreateOfferDto offer, int userId, CancellationToken cancellationToken);
        Task DeleteOfferAsync(int userId, int offerId, CancellationToken cancellationToken, bool moderator = false);
        Task EditOfferAsync(int userId, EditOfferDto dto, CancellationToken cancellationToken);
        Task<List<GetOfferDto>> GetAllOffersAsync(CancellationToken cancellationToken);
        Task<GetOfferDto?> GetOfferByIdAsync(int id, CancellationToken cancellationToken);
        Task<UserDto> GetOfferOwnerAsync(int id, CancellationToken cancellationToken);
        Task<List<GetOfferDto>> GetOfferBySalesPointIdAsync(int id, CancellationToken cancellationToken);
        Task<List<GetOfferDto>> GetOffersByFuelTypeAsync(FuelType fuelType, CancellationToken cancellationToken);
        Task<List<GetOfferDto>> GetOffersByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task AddCommentAsync(int userId, CreateOfferCommentDto dto, CancellationToken cancellationToken);
        Task DeleteCommentAsync(int userId, int commentId, CancellationToken cancellationToken, bool moderator = false);
        Task EditCommentAsync(int userId, EditOfferCommentDto dto, CancellationToken cancellationToken);
        Task<List<GetOfferCommentDto>> GetCommentsByOfferIdAsync(int offerId, CancellationToken cancellationToken);
    }
}