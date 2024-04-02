using FuelMarketplace.Domain.Models;
using FuelMarketplace.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FuelMarketplace.API.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult GetClaims()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var r = Enum.Parse<Role>(role);
            ClaimsDto claims = new ClaimsDto
            {
                Id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
                Email = User.FindFirstValue(ClaimTypes.Email)!,
                Role = Enum.Parse<Role>(User.FindFirstValue(ClaimTypes.Role)!)
            };
            return Ok(claims);
        }
    }
}
