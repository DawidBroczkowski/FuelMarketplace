﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelMarketplace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserBans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "Users");
        }
    }
}
