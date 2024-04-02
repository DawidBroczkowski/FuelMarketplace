using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelMarketplace.Domain.Models
{
    public record Post
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
        public DateTime Created { get; set; } = DateTime.Now;
        [AllowNull]
        public DateTime Updated { get; set; }
        [AllowNull]
        public FuelType? FuelType { get; set; }
        [AllowNull]
        public Address? Address { get; set; } = new Address();
        [AllowNull]
        public virtual List<Image> Images { get; set; } = new List<Image>();
        [Required]
        public virtual User User { get; set; }
        [AllowNull]
        public virtual List<PostComment> Comments { get; set; } = new List<PostComment>();
    }
}
