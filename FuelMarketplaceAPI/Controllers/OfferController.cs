using FuelMarketplace.Application.Interfaces;
using FuelMarketplace.Domain.Models;
using FuelMarketplace.Shared.Dtos.OfferCommentDtos;
using FuelMarketplace.Shared.Dtos.OfferDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FuelMarketplace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOfferService _offerService;

        public OfferController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _offerService = _serviceProvider.GetRequiredService<IOfferService>();
        }

        [HttpGet]
        public async Task<ActionResult<List<GetOfferDto>>> GetAllOffers(CancellationToken cancellationToken)
        {
            return Ok(await _offerService.GetAllOffersAsync(cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetOfferDto>> GetOfferById(int id, CancellationToken cancellationToken)
        {
            return Ok(await _offerService.GetOfferByIdAsync(id, cancellationToken));
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<List<GetOfferDto>>> GetOffersByUserId(int id, CancellationToken cancellationToken)
        {
            return Ok(await _offerService.GetOffersByUserIdAsync(id, cancellationToken));
        }

        [HttpGet("salespoint/{id}")]
        public async Task<ActionResult<GetOfferDto>> GetOfferBySalesPointId(int id, CancellationToken cancellationToken)
        {
            return Ok(await _offerService.GetOfferBySalesPointIdAsync(id, cancellationToken));
        }

        [HttpGet("fueltype/{type}")]
        public async Task<ActionResult<List<GetOfferDto>>> GetOffersByFuelTypeId(FuelType type, CancellationToken cancellationToken)
        {
            return Ok(await _offerService.GetOffersByFuelTypeAsync(type, cancellationToken));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddOffer(CreateOfferDto dto, CancellationToken cancellationToken)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _offerService.AddOfferAsync(dto, userId, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteOffer(int id, CancellationToken cancellationToken)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _offerService.DeleteOfferAsync(userId, id, cancellationToken);
            return Ok();
        }

        [HttpDelete("admin/{id}")]
        [Authorize(Policy = "AdminOrMod")]
        public async Task<ActionResult> DeleteOfferAdmin(int id, CancellationToken cancellationToken)
        {
            await _offerService.DeleteOfferAsync(-1, id, cancellationToken, true);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> EditOffer(EditOfferDto dto, CancellationToken cancellationToken)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _offerService.EditOfferAsync(userId, dto, cancellationToken);
            return Ok();
        }

        [HttpPost("comment")]
        [Authorize]
        public async Task<ActionResult> AddComment(CreateOfferCommentDto dto, CancellationToken cancellationToken)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _offerService.AddCommentAsync(userId, dto, cancellationToken);
            return Ok();
        }

        [HttpDelete("comment/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteComment(int id, CancellationToken cancellationToken)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _offerService.DeleteCommentAsync(userId, id, cancellationToken);
            return Ok();
        }

        [HttpDelete("comment/admin/{id}")]
        [Authorize(Policy = "AdminOrMod")]
        public async Task<ActionResult> DeleteCommentAdmin(int id, CancellationToken cancellationToken)
        {
            await _offerService.DeleteCommentAsync(-1, id, cancellationToken, true);
            return Ok();
        }

        [HttpPut("comment")]
        [Authorize]
        public async Task<ActionResult> EditComment(EditOfferCommentDto dto, CancellationToken cancellationToken)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _offerService.EditCommentAsync(userId, dto, cancellationToken);
            return Ok();
        }

        [HttpGet("comments/{id}")]
        public async Task<ActionResult<List<GetOfferCommentDto>>> GetCommentsByOfferId(int id, CancellationToken cancellationToken)
        {
            return Ok(await _offerService.GetCommentsByOfferIdAsync(id, cancellationToken));
        }
    }
}
