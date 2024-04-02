using FuelMarketplace.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelMarketplace.Shared.Dtos.OfferCommentDtos
{
    public record CreateOfferCommentDto
    {
        [Required]
        [StringLength(5000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int OfferId { get; set; }
    }
}
