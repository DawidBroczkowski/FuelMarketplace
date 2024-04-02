using FuelMarketplace.Domain.Models;

namespace FuelMarketplace.Shared.Dtos.OfferDtos
{
    public record GetOfferDto
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public FuelType FuelType { get; init; }
        public int? SalesPointId { get; init; }
        public int UserId { get; init; }
        public List<Image> Images { get; init; } = new();
        public Address Address { get; init; } = new();
        public DateTime Created { get; init; }
        public DateTime? Updated { get; init; }
    }
}
