using FuelMarketplace.Domain.Models;

namespace FuelMarketplace.Application.Interfaces
{
    public interface IImageService
    {
        bool CheckIfImageExists(string fileName);
        bool CheckIfImageExistsByGuid(Guid fileId);
        Task SaveImageAsync(int userId, int objectId, Stream fileStream, ImageCategory category, CancellationToken cancellationToken);
        Task DeleteImageAsync(int userId, int objectId, Guid fileGuid, ImageCategory category, CancellationToken cancellationToken);
        Stream GetImage(string fileName);
        Stream GetImageByGuid(Guid fileGuid);
        int GetImageOwnerId(Guid fileGuid);
    }
}