using FuelMarketplace.Domain.Models;

namespace FuelMarketplace.Shared.Dtos
{
    public record UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public Role Role { get; set; }
        public bool IsBanned { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid ProfileImageGuid { get; set; }
    }
}
