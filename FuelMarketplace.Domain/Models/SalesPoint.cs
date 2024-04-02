using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FuelMarketplace.Domain.Models
{
    public record SalesPoint
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(5), MaxLength(1000)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        [AllowNull]
        public Image? ProfileImage { get; set; }
        [AllowNull]
        public virtual List<Image> Images { get; set; } = new List<Image>();
        [Required]
        public Address Address { get; set; } = new Address();
        [AllowNull]
        public virtual List<Offer> Offers { get; set; } = new List<Offer>();
        [Required]
        public virtual User? User { get; set; }
    }
}
