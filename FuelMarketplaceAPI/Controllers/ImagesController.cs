using Microsoft.AspNetCore.Mvc;
using FuelMarketplace.Domain.Models;
using FuelMarketplace.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FuelMarketplace.Application.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly IImageService _imageService;
    private readonly IServiceProvider _serviceProvider;


    public ImagesController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _imageService = _serviceProvider.GetRequiredService<IImageService>();
    }

    [HttpPost("{category}/{objectId}")]
    [Consumes("multipart/form-data")]
    [Authorize]
    public async Task<ActionResult> UploadImage(IFormFile file, ImageCategory category, int objectId, CancellationToken cancellationToken)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _imageService.SaveImageAsync(userId, objectId, file.OpenReadStream(), category, cancellationToken);
        return Ok();
    }

    [HttpGet("{fileId}")]
    public ActionResult GetImage(Guid fileId)
    {
        return File(_imageService.GetImageByGuid(fileId), "image/png");
    }

    [HttpGet("exists/{fileId}")]
    public ActionResult CheckIfImageExists(Guid fileId)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        string fileName = $"{userId}_{fileId}";
        return Ok(_imageService.CheckIfImageExists(fileName));
    }

    [HttpDelete()]
    [Authorize]
    public async Task<ActionResult> DeleteImage(Guid fileId, ImageCategory category, int objectId, CancellationToken cancellationToken)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _imageService.DeleteImageAsync(userId, objectId, fileId, category, cancellationToken);
        return Ok();
    }

    [HttpDelete("admin")]
    [Authorize(Roles = "AdminOrMod")]
    public async Task<ActionResult> AdminDeleteImage(Guid fileGuid, ImageCategory category, int objectId, CancellationToken cancellationToken)
    {
        int userId = _imageService.GetImageOwnerId(fileGuid);
        await _imageService.DeleteImageAsync(userId, objectId, fileGuid, category, cancellationToken); ;
        return Ok();
    }
}
