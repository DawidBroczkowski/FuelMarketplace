using FuelMarketplace.Application.Interfaces;
using FuelMarketplace.Infrastructure.DataAccess.Interfaces;
using FuelMarketplace.Shared.Dtos.SalesPointDtos;
using FuelMarketplace.Shared.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace FuelMarketplace.Application.Services
{
    public class SalesPointService : ISalesPointService
    {
        private readonly ISalesPointRepository _salesPointRepository;
        private readonly IUserRepository _userRepository;

        public SalesPointService(ISalesPointRepository salesPointRepository, IUserRepository userRepository)
        {
            _salesPointRepository = salesPointRepository;
            _userRepository = userRepository;
        }

        public async Task AddSalesPointAsync(CreateSalesPointDto dto, int userId, CancellationToken cancellationToken)
        {
            await _salesPointRepository.AddSalesPointAsync(dto, userId, cancellationToken);
        }

        public async Task<GetSalesPointDto> GetSalesPointByIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _salesPointRepository.GetSalesPointByIdAsync(id, cancellationToken);
            if (result is null)
            {
                var ex = new KeyNotFoundException("Can't find sales point.");
                ex.Data.Add("Id", "Sales point not found.");
                throw ex;
            }
            return result;
        }

        public async Task<List<GetSalesPointDto>> GetAllSalesPointsAsync(CancellationToken cancellationToken)
        {
            return await _salesPointRepository.GetAllSalesPointsAsync(cancellationToken);
        }

        public async Task DeleteSalesPointAsync(int userId, int salesPointId, CancellationToken cancellationToken, bool moderator = false)
        {
            var salesPointExists = await _salesPointRepository.CheckIfSalesPointExistsAsync(salesPointId, cancellationToken);
            if (salesPointExists is false)
            {
                var ex = new KeyNotFoundException("Can't find sales point.");
                ex.Data.Add("Id", "Sales point not found.");
                throw ex;
            }
            if (await _salesPointRepository.CheckIfSalesPointBelongsToUserAsync(salesPointId, userId, cancellationToken) is false
                                              && moderator is false)
            {
                var ex = new AuthorizationException("User is not owner.");
                ex.Data.Add("User", "User is not the owner of this sales point.");
                throw ex;
            }
            await _salesPointRepository.DeleteSalesPointByIdAsync(salesPointId, cancellationToken);
        }

        public async Task EditSalesPointAsync(int userId, EditSalesPointDto dto, CancellationToken cancellationToken)
        {
            if (await _salesPointRepository.CheckIfSalesPointExistsAsync(dto.Id, cancellationToken) is false)
            {
                var ex = new KeyNotFoundException("Can't find sales point.");
                ex.Data.Add("Id", "Sales point not found.");
                throw ex;
            }
            if (await _salesPointRepository.CheckIfSalesPointBelongsToUserAsync(userId, dto.Id, cancellationToken) is false)
            {
                var ex = new AuthorizationException("User is not owner.");
                ex.Data.Add("User", "User is not the owner of this sales point.");
                throw ex;
            }
            await _salesPointRepository.UpdateSalesPointAsync(dto, cancellationToken);
        }

        public async Task<List<GetSalesPointDto>> GetSalesPointsByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var userExists = await _userRepository.CheckIfUserExistsAsync(userId, cancellationToken);
            if (userExists is false)
            {
                var ex = new KeyNotFoundException("Can't find user.");
                ex.Data.Add("Id", "User not found.");
                throw ex;
            }
            return await _salesPointRepository.GetSalesPointsByUserIdAsync(userId, cancellationToken);
        }

    }
}
