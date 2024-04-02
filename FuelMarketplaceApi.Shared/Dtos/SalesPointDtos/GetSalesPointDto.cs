using FuelMarketplace.Domain.Models;

namespace FuelMarketplace.Shared.Dtos.SalesPointDtos
{
    public record GetSalesPointDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public Image? ProfileImage { get; init; }
        public List<Image> Images { get; init; } = new();
        public Address Address { get; init; } = new();
        public int UserId { get; init; }
    }
}
