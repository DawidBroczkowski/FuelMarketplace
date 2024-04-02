using FuelMarketplace.Domain.Models;
using FuelMarketplace.Shared.Dtos.SalesPointDtos;
using System.Threading;

namespace FuelMarketplace.Infrastructure.DataAccess.Interfaces
{
    public interface ISalesPointRepository
    {
        Task AddSalesPointAsync(CreateSalesPointDto dto, int userId, CancellationToken cancellationToken);
        Task<bool> CheckIfOfferBelongsToSalesPointAsync(int salesPointId, int offerId, CancellationToken cancellationToken);
        Task<bool> CheckIfSalesPointBelongsToUserAsync(int salesPointId, int userId, CancellationToken cancellationToken);
        Task<bool> CheckIfSalesPointExistsAsync(int id, CancellationToken cancellationToken);
        Task DeleteSalesPointAsync(SalesPoint salesPoint, CancellationToken cancellationToken);
        Task DeleteSalesPointByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<GetSalesPointDto>> GetAllSalesPointsAsync(CancellationToken cancellationToken);
        Task<GetSalesPointDto?> GetSalesPointByIdAsync(int id, CancellationToken cancellationToken);
        Task<GetSalesPointDto?> GetSalesPointByOfferIdAsync(int offerId, CancellationToken cancellationToken);
        Task<List<GetSalesPointDto>> GetSalesPointsByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task UpdateSalesPointAsync(EditSalesPointDto dto, CancellationToken cancellationToken);
        Task AddImageAsync(int salesPointId, Guid fileGuid, CancellationToken cancellationToken);
        Task DeleteImageAsync(int salesPointId, Guid fileGuid, CancellationToken cancellationToken);
    }
}