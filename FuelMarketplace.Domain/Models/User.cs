using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace FuelMarketplace.Domain.Models
{
    public record User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Surname { get; set; } = string.Empty;

        [Required]
        [MaxLength(320)]
        [Column(TypeName = "varchar(320)")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string AccountName { get; set; } = string.Empty;

        [Required]
        public Role Role { get; set; } = Role.user;

        [Required]
        public bool IsBanned { get; set; } = false;

        [AllowNull]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(64)]
        public byte[]? PasswordHash { get; set; }

        [Required]
        [MaxLength(128)]
        public byte[]? PasswordSalt { get; set; }

        [AllowNull]
        public virtual Image? ProfileImage { get; set; }

        [AllowNull]
        public virtual List<SalesPoint> SalesPoints { get; set; } = new List<SalesPoint>();

        [AllowNull]
        public virtual List<Offer> Offers { get; set; } = new List<Offer>();

        [AllowNull]
        public virtual List<Post> Posts { get; set; } = new List<Post>();

        [AllowNull]
        public virtual List<PostComment> PostComments { get; set; } = new List<PostComment>();

        [AllowNull]
        public virtual List<OfferComment> OfferComments { get; set; } = new List<OfferComment>();
    }
}
