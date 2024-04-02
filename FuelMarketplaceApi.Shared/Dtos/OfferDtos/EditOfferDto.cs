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
    public record EditOfferDto
    {
        [Required]
        public int Id { get; init; }
        [AllowNull]
        [MinLength(5), MaxLength(1000)]
        public string? Title { get; init; }
        [AllowNull]
        [MaxLength(10000)]
        public string? Description { get; init; }
        [AllowNull]
        public decimal? Price { get; init; }
        [AllowNull]
        public FuelType? FuelType { get; init; }
        [AllowNull]
        public int? SalesPointId { get; init; }
    }
}
