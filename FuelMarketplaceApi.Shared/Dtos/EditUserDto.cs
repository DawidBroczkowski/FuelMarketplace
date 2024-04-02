using FuelMarketplace.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelMarketplace.Shared.Dtos
{
    public record EditUserDto
    {
        [Required]
        public int Id { get; init; }

        [AllowNull]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [AllowNull]
        public virtual Guid? ProfileImageGuid { get; set; }
    }
}
