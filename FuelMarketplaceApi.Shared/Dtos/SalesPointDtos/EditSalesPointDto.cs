using FuelMarketplace.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelMarketplace.Shared.Dtos.SalesPointDtos
{
    public record EditSalesPointDto
    {
        [Required]
        public int Id { get; init; }
        [AllowNull]
        [MinLength(5), MaxLength(1000)]
        public string? Name { get; init; }
        [AllowNull]
        [MaxLength(1000)]
        public string? Description { get; init; }
    }
}
