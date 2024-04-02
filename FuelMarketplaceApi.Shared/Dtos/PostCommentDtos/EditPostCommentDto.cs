using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelMarketplace.Shared.Dtos.PostCommentDtos
{
    public record EditPostCommentDto
    {
        [Required]
        [StringLength(10000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int Id { get; set; }
    }
}
