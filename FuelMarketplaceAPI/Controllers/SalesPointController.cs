using FuelMarketplace.Application.Interfaces;
using FuelMarketplace.Shared.Dtos.SalesPointDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FuelMarketplace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesPointController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISalesPointService _salesPointService;

        public SalesPointController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _salesPointService = _serviceProvider.GetRequiredService<ISalesPointService>();
        }

        [HttpGet]
        public async Task<ActionResult<List<GetSalesPointDto>>> GetAllSalesPoints(CancellationToken cancellationToken)
        {
            return Ok(await _salesPointService.GetAllSalesPointsAsync(cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetSalesPointDto>> GetSalesPointById(int id, CancellationToken cancellationToken)
        {
            return Ok(await _salesPointService.GetSalesPointByIdAsync(id, cancellationToken));
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<List<GetSalesPointDto>>> GetSalesPointsByUserId(int id, CancellationToken cancellationToken)
        {
            return Ok(await _salesPointService.GetSalesPointsByUserIdAsync(id, cancellationToken));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddSalesPoint(CreateSalesPointDto dto, CancellationToken cancellationToken)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _salesPointService.AddSalesPointAsync(dto, userId, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteSalesPoint(int id, CancellationToken cancellationToken)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _salesPointService.DeleteSalesPointAsync(userId, id, cancellationToken);
            return Ok();
        }

        [HttpDelete("admin/{id}")]
        [Authorize(Policy = "AdminOrMod")]
        public async Task<ActionResult> DeleteSalesPointAdmin(int id, CancellationToken cancellationToken)
        {
            await _salesPointService.DeleteSalesPointAsync(-1, id, cancellationToken, true);
            return Ok();
        }
    }
}
