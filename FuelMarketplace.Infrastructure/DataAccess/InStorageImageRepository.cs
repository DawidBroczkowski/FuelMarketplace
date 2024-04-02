using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;

namespace FuelMarketplace.Infrastructure.DataAccess
{
    public class InStorageImageRepository : IImageRepository
    {
        private readonly string _storagePath = "images";

        public async Task SaveImageAsync(string fileName, Stream fileStream)
        {
            var filePath = Path.Combine(_storagePath, $"{fileName}.png");
            await using var output = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(output);
        }

        public Stream GetImageStream(string fileName)
        {
            var filePath = Path.Combine(_storagePath, fileName.ToString());
            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        public Stream GetImageStreamByGuid(Guid fileId)
        {
            var fileNameContains = $"*_{fileId}.*";
            var matchingFiles = Directory.EnumerateFiles(_storagePath, fileNameContains);
            var filePath = matchingFiles.First();
            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        public void DeleteImage(string fileName)
        {
            var filePath = Path.Combine(_storagePath, fileName.ToString());
            File.Delete(filePath);
        }

        public bool CheckIfImageExists(string fileName)
        {
            var filePath = Path.Combine(_storagePath, fileName.ToString());
            return File.Exists(filePath);
        }

        public bool CheckIfImageExistsByGuid(Guid fileId)
        {
            var fileNameContains = $"*_{fileId}.*";
            var matchingFiles = Directory.EnumerateFiles(_storagePath, fileNameContains);

            return matchingFiles.Any();
        }

        public int GetImageOwnerId(Guid fileGuid)
        {
            var fileNameContains = $"*_{fileGuid}.*";
            var matchingFiles = Directory.EnumerateFiles(_storagePath, fileNameContains);
            var filePath = matchingFiles.First();
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var userId = fileName.Split('_')[0];
            return int.Parse(userId);
        }
    }
}
