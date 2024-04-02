using FuelMarketplace.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelMarketplace.Shared.Dtos.PostDtos
{
    public record EditPostDto
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
        public FuelType? FuelType { get; init; }
        [AllowNull]
        public Address? Address { get; init; }
    }
}
