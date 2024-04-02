using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelMarketplace.Domain.Models
{
    public record OfferComment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(5000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public virtual User User { get; set; }

        [Required]
        public virtual Offer Offer { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;
        [AllowNull]
        public DateTime? Updated { get; set; }
    }
}
