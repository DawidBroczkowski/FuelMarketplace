using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelMarketplace.Domain.Models
{
    public record Offer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5), MaxLength(1000)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(10000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [AllowNull]
        public DateTime Updated { get; set; }

        [Required]
        public FuelType FuelType { get; set; }

        [Required]
        public Address Address { get; set; } = new Address();

        [AllowNull]
        public virtual List<Image> Images { get; set; } = new List<Image>();

        [Required]
        public virtual User User { get; init; }

        [AllowNull]
        public virtual SalesPoint? SalesPoint { get; set; }

        [AllowNull]
        public virtual List<OfferComment> Comments { get; set; } = new List<OfferComment>();
    }
}
