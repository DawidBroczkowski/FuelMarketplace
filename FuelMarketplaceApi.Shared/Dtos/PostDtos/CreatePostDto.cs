using FuelMarketplace.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FuelMarketplace.Shared.Dtos.PostDtos
{
    public record CreatePostDto
    {
        [Required]
        [MinLength(5), MaxLength(1000)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(10000)]
        public string Description { get; set; } = string.Empty;
        [AllowNull]
        public FuelType? FuelType { get; set; }
        [AllowNull]
        public Address? Address { get; set; } = new Address();
    }
}
