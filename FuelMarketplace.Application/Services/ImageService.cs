using FuelMarketplace.Application.Interfaces;
using FuelMarketplace.Domain.Models;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using FuelMarketplace.Shared.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace FuelMarketplace.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IImageRepository _imageRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISalesPointRepository _salesPointRepository;
        private readonly IOfferRepository _orderRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        public ImageService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _imageRepository = _serviceProvider.GetRequiredService<IImageRepository>();
            _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();
            _salesPointRepository = _serviceProvider.GetRequiredService<ISalesPointRepository>();
            _orderRepository = _serviceProvider.GetRequiredService<IOfferRepository>();
            _postRepository = _serviceProvider.GetRequiredService<IPostRepository>();
            _commentRepository = _serviceProvider.GetRequiredService<ICommentRepository>();
        }

        public Stream GetImage(string fileName)
        {
            if (_imageRepository.CheckIfImageExists(fileName) is false)
            {
                FileNotFoundException ex = new FileNotFoundException("Image not found");
                ex.Data.Add("fileName", fileName);
                throw ex;
            }
            return _imageRepository.GetImageStream(fileName);
        }

        public Stream GetImageByGuid(Guid guid)
        {
            if (_imageRepository.CheckIfImageExistsByGuid(guid) is false)
            {
                FileNotFoundException ex = new FileNotFoundException("Image not found");
                ex.Data.Add("guid", guid);
                throw ex;
            }
            return _imageRepository.GetImageStreamByGuid(guid);
        }

        public async Task SaveImageAsync(int userId, int objectId, Stream fileStream, ImageCategory category, CancellationToken cancellationToken)
        {
            Guid fileGuid = Guid.NewGuid();
            string fileName = $"{userId}_{fileGuid}";
            await _imageRepository.SaveImageAsync(fileName, fileStream);

            switch (category)
            {
                case ImageCategory.user:
                    await _userRepository.UpdateUserAsync(new EditUserDto { Id = userId, ProfileImageGuid = fileGuid }, cancellationToken);
                    break;

                case ImageCategory.salesPoint:
                    if (await _salesPointRepository.CheckIfSalesPointExistsAsync(objectId, cancellationToken) is false)
                    {
                        KeyNotFoundException ex = new KeyNotFoundException("Sales point not found");
                        ex.Data.Add("Id", objectId);
                        throw ex;
                    }
                    if (await _salesPointRepository.CheckIfSalesPointBelongsToUserAsync(objectId, userId, cancellationToken) is false)
                    {
                        UnauthorizedAccessException ex = new UnauthorizedAccessException("Sales point does not belong to user");
                        ex.Data.Add("Id", objectId);
                        throw ex;
                    }
                    await _salesPointRepository.AddImageAsync(objectId, fileGuid, cancellationToken);
                    break;

                case ImageCategory.offer:
                    if (await _orderRepository.CheckIfOfferExistsAsync(objectId, cancellationToken) is false)
                    {
                        KeyNotFoundException ex = new KeyNotFoundException("Offer not found");
                        ex.Data.Add("Id", objectId);
                        throw ex;
                    }
                    if (await _orderRepository.CheckIfOfferBelongsToUserAsync(objectId, userId, cancellationToken) is false)
                    {
                        UnauthorizedAccessException ex = new UnauthorizedAccessException("Offer does not belong to user");
                        ex.Data.Add("Id", objectId);
                        throw ex;
                    }
                    await _orderRepository.AddImageAsync(objectId, fileGuid, cancellationToken);
                    break;

                case ImageCategory.post:
                    if (await _postRepository.CheckIfPostExistsAsync(objectId, cancellationToken) is false)
                    {
                        KeyNotFoundException ex = new KeyNotFoundException("Post not found");
                        ex.Data.Add("Id", objectId);
                        throw ex;
                    }
                    if (await _postRepository.CheckIfPostBelongsToUserAsync(objectId, userId, cancellationToken) is false)
                    {
                        UnauthorizedAccessException ex = new UnauthorizedAccessException("Post does not belong to user");
                        ex.Data.Add("Id", objectId);
                        throw ex;
                    }
                    await _postRepository.AddImageAsync(objectId, fileGuid, cancellationToken);
                    break;
            }
        }

        public async Task DeleteImageAsync(int userId, int objectId, Guid fileGuid, ImageCategory category, CancellationToken cancellationToken)
        {
            string fileName = $"{userId}_{fileGuid}";

            if (_imageRepository.CheckIfImageExists(fileName) is false)
            {
                FileNotFoundException ex = new FileNotFoundException("Image not found");
                ex.Data.Add("fileName", fileName);
                throw ex;
            }

            switch (category)
            {
                case ImageCategory.user:
                    await _userRepository.UpdateUserAsync(new EditUserDto { Id = userId, ProfileImageGuid = null }, cancellationToken);
                    break;

                case ImageCategory.salesPoint:
                    if (await _salesPointRepository.CheckIfSalesPointExistsAsync(objectId, cancellationToken) is false)
                    {
                        KeyNotFoundException ex = new KeyNotFoundException("Sales point not found");
                        ex.Data.Add("Id", objectId);
                        throw ex;
                    }
                    if (await _salesPointRepository.CheckIfSalesPointBelongsToUserAsync(objectId, userId, cancellationToken) is false)
                    {
                        UnauthorizedAccessException ex = new UnauthorizedAccessException("Sales point does not belong to user");
                        ex.Data.Add("Id", objectId);
                        throw ex;
                    }
                    await _salesPointRepository.DeleteImageAsync(objectId, fileGuid, cancellationToken);
                    break;

                case ImageCategory.offer:
                    if (await _orderRepository.CheckIfOfferExistsAsync(objectId, cancellationToken) is false)
                    {
                        KeyNotFoundException ex = new KeyNotFoundException("Offer not found");
                        ex.Data.Add("Id", objectId);
                        throw ex;
                    }
                    if (await _orderRepository.CheckIfOfferBelongsToUserAsync(objectId, userId, cancellationToken) is false)
                    {
                        UnauthorizedAccessException ex = new UnauthorizedAccessException("Offer does not belong to user");
                        ex.Data.Add("Id", objectId);
                        throw ex;
                    }
                    await _orderRepository.DeleteImageAsync(objectId, fileGuid, cancellationToken);
                    break;

                case ImageCategory.post:
                    if (await _postRepository.CheckIfPostExistsAsync(objectId, cancellationToken) is false)
                    {
                        KeyNotFoundException ex = new KeyNotFoundException("Post not found");
                        ex.Data.Add("Id", objectId);
                        throw ex;
                    }
                    if (await _postRepository.CheckIfPostBelongsToUserAsync(objectId, userId, cancellationToken) is false)
                    {
                        UnauthorizedAccessException ex = new UnauthorizedAccessException("Post does not belong to user");
                        ex.Data.Add("Id", objectId);
                        throw ex;
                    }
                    await _postRepository.DeleteImageAsync(objectId, fileGuid, cancellationToken);
                    break;
                case ImageCategory.comment:
                    if (await _commentRepository.CheckIfPostCommentExistsAsync(objectId, cancellationToken) is false)
                    {
                        KeyNotFoundException ex = new KeyNotFoundException("Comment not found");
                        ex.Data.Add("Id", objectId);
                        throw ex;
                    }
                    if (await _commentRepository.CheckIfPostCommentBelongsToUserAsync(objectId, userId, cancellationToken) is false)
                    {
                        UnauthorizedAccessException ex = new UnauthorizedAccessException("Comment does not belong to user");
                        ex.Data.Add("Id", objectId);
                        throw ex;
                    }
                    await _commentRepository.DeletePostCommentImageAsync(objectId, fileGuid, cancellationToken);
                    break;
            }

            _imageRepository.DeleteImage(fileName);
        }

        public bool CheckIfImageExists(string fileName)
        {
            return _imageRepository.CheckIfImageExists(fileName);
        }

        public bool CheckIfImageExistsByGuid(Guid fileId)
        {
            return _imageRepository.CheckIfImageExistsByGuid(fileId);
        }

        public int GetImageOwnerId(Guid fileGuid)
        {
            return _imageRepository.GetImageOwnerId(fileGuid);
        }
    }
}
