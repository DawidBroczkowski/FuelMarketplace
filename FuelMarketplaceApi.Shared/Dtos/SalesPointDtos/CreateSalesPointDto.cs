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
    public record CreateSalesPointDto
    {
        [Required]
        [MinLength(5), MaxLength(1000)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public Address Address { get; set; } = new Address();
    }
}
