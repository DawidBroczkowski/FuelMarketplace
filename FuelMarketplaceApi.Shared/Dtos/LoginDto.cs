﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelMarketplace.Shared.Dtos
{
    public record LoginDto
    {
        [Required]
        [MinLength(3), MaxLength(320)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6), MaxLength(30)]
        public string Password { get; set; } = string.Empty;
    }
}
