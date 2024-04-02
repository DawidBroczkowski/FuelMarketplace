using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FuelMarketplace.Domain.Models
{
    public record Address
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Voivodeship Voivodeship { get; set; }
        [Required]
        public string City { get; set; } = string.Empty;
        [AllowNull]
        [MaxLength(200)]
        public string Street { get; set; } = string.Empty;
        [AllowNull]
        [MaxLength(10)]
        public string PostalCode { get; set; } = string.Empty;
        [AllowNull]
        public double Latitude { get; set; }
        [AllowNull]
        public double Longitude { get; set; }
    }
}
