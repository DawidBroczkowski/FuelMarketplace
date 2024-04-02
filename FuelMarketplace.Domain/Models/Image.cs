using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelMarketplace.Domain.Models
{
    public record Image
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid FileGuid { get; set; }
    }
}
