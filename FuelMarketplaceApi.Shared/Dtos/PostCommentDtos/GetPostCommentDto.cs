using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelMarketplace.Shared.Dtos.PostCommentDtos
{
    public record GetPostCommentDto
    {
        public int Id { get; init; }
        public string Description { get; init; } = string.Empty;
        public DateTime Created { get; init; }
        public DateTime? Updated { get; init; }
        public int UserId { get; init; }
    }
}
