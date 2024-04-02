using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelMarketplace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AccountTypeToRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountType",
                table: "Users",
                newName: "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Users",
                newName: "AccountType");
        }
    }
}
