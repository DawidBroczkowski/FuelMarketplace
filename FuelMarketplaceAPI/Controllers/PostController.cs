using FuelMarketplace.Application.Interfaces;
using FuelMarketplace.Domain.Models;
using FuelMarketplace.Shared.Dtos.PostCommentDtos;
using FuelMarketplace.Shared.Dtos.PostDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FuelMarketplace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPostService _postService;

        public PostController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _postService = _serviceProvider.GetRequiredService<IPostService>();
        }

        [HttpGet]
        public async Task<ActionResult<List<GetPostDto>>> GetAllPostsAsync(CancellationToken cancellationToken)
        {
            return Ok(await _postService.GetAllPostsAsync(cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetPostDto>> GetPostByIdAsync(int id, CancellationToken cancellationToken)
        {
            return Ok(await _postService.GetPostByIdAsync(id, cancellationToken));
        }

        [HttpPost]
        public async Task<ActionResult> AddPostAsync(CreatePostDto dto, CancellationToken cancellationToken)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); 
            await _postService.AddPostAsync(dto, userId, cancellationToken);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> EditPostAsync(EditPostDto dto, CancellationToken cancellationToken)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _postService.EditPostAsync(userId, dto, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePostAsync(int id, CancellationToken cancellationToken)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _postService.DeletePostAsync(userId, id, cancellationToken);
            return Ok();
        }

        [HttpDelete("admin/{id}")]
        [Authorize(Policy = "AdminOrMod")]
        public async Task<ActionResult> DeletePostAdminAsync(int id, CancellationToken cancellationToken)
        {
            await _postService.DeletePostAsync(-1, id, cancellationToken, true);
            return Ok();
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<GetPostDto>>> GetPostsByUserIdAsync(int id, CancellationToken cancellationToken)
        {
            return Ok(await _postService.GetPostsByUserIdAsync(id, cancellationToken));
        }

        [HttpGet("fueltype/{type}")]
        public async Task<ActionResult<IEnumerable<GetPostDto>>> GetPostsByFuelTypeAsync(FuelType type, CancellationToken cancellationToken)
        {
            return Ok(await _postService.GetPostsByFuelTypeAsync(type, cancellationToken));
        }

        [HttpPost("comment")]
        [Authorize]
        public async Task<ActionResult> AddCommentAsync(CreatePostCommentDto dto, CancellationToken cancellationToken)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _postService.AddCommentAsync(userId, dto, cancellationToken);
            return Ok();
        }

        [HttpDelete("comment/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteCommentAsync(int id, CancellationToken cancellationToken)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _postService.DeleteCommentAsync(userId, id, cancellationToken);
            return Ok();
        }

        [HttpDelete("admin/comment/{id}")]
        [Authorize(Policy = "AdminOrMod")]
        public async Task<ActionResult> DeleteCommentAdminAsync(int id, CancellationToken cancellationToken)
        {
            await _postService.DeleteCommentAsync(-1, id, cancellationToken, true);
            return Ok();
        }

        [HttpPut("comment")]
        [Authorize]
        public async Task<ActionResult> EditCommentAsync(EditPostCommentDto dto, CancellationToken cancellationToken)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _postService.EditCommentAsync(userId, dto, cancellationToken);
            return Ok();
        }

        [HttpGet("comments/{id}")]
        public async Task<ActionResult<List<GetPostCommentDto>>> GetCommentsByPostIdAsync(int id, CancellationToken cancellationToken)
        {
            return Ok(await _postService.GetCommentsByPostIdAsync(id, cancellationToken));
        }
    }
}
