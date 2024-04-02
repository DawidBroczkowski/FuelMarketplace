using FuelMarketplace.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelMarketplace.Shared.Dtos
{
    public record CredentialsDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public Role Role { get; set; }
        public bool IsBanned { get; set; }
    }
}
