using FuelMarketplace.Application.Interfaces;
using FuelMarketplace.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FuelMarketplace.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private IServiceProvider _serviceProvider;
        private IAccountService _accountService;

        public AccountController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _accountService = _serviceProvider.GetRequiredService<IAccountService>();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<ValidationResult>> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken)
        {
            await _accountService.RegisterUserAsync(registerDto, cancellationToken);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
        {
            string token = await _accountService.LoginUserAsync(loginDto, cancellationToken);
            return Ok(token);
        }
    }
}
