namespace FuelMarketplace.Infrastructure.DataAccess.Interfaces
{
    public interface IImageRepository
    {
        bool CheckIfImageExists(string fileName);
        bool CheckIfImageExistsByGuid(Guid fileId);
        void DeleteImage(string fileName);
        Stream GetImageStream(string fileName);
        Stream GetImageStreamByGuid(Guid fileId);
        Task SaveImageAsync(string fileName, Stream fileStream);
        int GetImageOwnerId(Guid fileGuid);
    }
}