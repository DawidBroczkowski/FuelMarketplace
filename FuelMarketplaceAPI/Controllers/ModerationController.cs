using FuelMarketplace.Application.Interfaces;
using FuelMarketplace.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuelMarketplace.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ModerationController : Controller
    {
        private IServiceProvider _serviceProvider;
        private IModerationService _moderationService;

        public ModerationController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _moderationService = _serviceProvider.GetRequiredService<IModerationService>();
        }

        [Authorize(Policy = "AdminOrMod")]
        [HttpPut("ban")]
        public async Task<ActionResult> BanUserAsync(string email, CancellationToken cancellationToken)
        {
            await _moderationService.BanUserAsync(email, cancellationToken);
            return Ok();
        }

        [Authorize(Policy = "AdminOrMod")]
        [HttpPut("unban")]
        public async Task<ActionResult> UnbanUserAsync(string email, CancellationToken cancellationToken)
        {
            await _moderationService.UnbanUserAsync(email, cancellationToken);
            return Ok();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("role")]
        public async Task<ActionResult> SetUserRoleAsync(string email, Role role, CancellationToken cancellationToken)
        {
            await _moderationService.SetUserRoleAsync(email, role, cancellationToken);
            return Ok();
        }
    }
}
