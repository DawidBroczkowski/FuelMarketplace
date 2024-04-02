using FuelMarketplace.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelMarketplace.Shared.Dtos.OfferDtos
{
    public record CreateOfferDto
    {
        [Required]
        [MinLength(5), MaxLength(1000)]
        public string Title { get; init; } = string.Empty;
        
        [Required]
        [MaxLength(10000)]
        public string Description { get; init; } = string.Empty;
        [Required]
        public decimal Price { get; init; }
        [Required]
        public FuelType FuelType { get; init; }
        [AllowNull]
        public int? SalesPointId { get; init; }
        [Required]
        public Address Address { get; init; } = new Address();
    }
}
