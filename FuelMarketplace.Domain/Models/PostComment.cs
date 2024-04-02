using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelMarketplace.Domain.Models
{
    public record PostComment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public virtual User User { get; set; }

        [Required]
        public virtual Post Post { get; set; }

        [AllowNull]
        public virtual List<Image> Images { get; set; } = new List<Image>();

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;
        [AllowNull]
        public DateTime? Updated { get; set; }
    }
}
