using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuelMarketplace.Shared.Dtos
{
    public record RegisterDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Surname { get; set; } = string.Empty;

        [Required]
        [MinLength(3), MaxLength(320)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6), MaxLength(30)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MinLength(3), MaxLength(20)]
        public string AccountName { get; set; } = string.Empty;
    }
}
