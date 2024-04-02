using FuelMarketplace.Shared.Dtos.SalesPointDtos;

namespace FuelMarketplace.Application.Interfaces
{
    public interface ISalesPointService
    {
        Task AddSalesPointAsync(CreateSalesPointDto dto, int userId, CancellationToken cancellationToken);
        Task DeleteSalesPointAsync(int userId, int salesPointId, CancellationToken cancellationToken, bool moderator = false);
        Task EditSalesPointAsync(int userId, EditSalesPointDto dto, CancellationToken cancellationToken);
        Task<List<GetSalesPointDto>> GetAllSalesPointsAsync(CancellationToken cancellationToken);
        Task<GetSalesPointDto> GetSalesPointByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<GetSalesPointDto>> GetSalesPointsByUserIdAsync(int userId, CancellationToken cancellationToken);
    }
}