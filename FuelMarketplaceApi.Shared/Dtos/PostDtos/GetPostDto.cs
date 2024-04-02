using FuelMarketplace.Domain.Models;

namespace FuelMarketplace.Shared.Dtos.PostDtos
{
    public record GetPostDto
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTime Created { get; init; }
        public DateTime Updated { get; init; }
        public FuelType? FuelType { get; init; }
        public Address? Address { get; init; }
        public List<Image> Images { get; init; } = new();
        public int UserId { get; init; }
    }
}
