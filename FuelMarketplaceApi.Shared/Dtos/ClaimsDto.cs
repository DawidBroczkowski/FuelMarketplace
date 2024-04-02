using FuelMarketplace.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelMarketplace.Shared.Dtos
{
    public class ClaimsDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public Role Role { get; set; }
    }
}
