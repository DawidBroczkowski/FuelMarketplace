using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelMarketplace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Images",
                newName: "FileGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileGuid",
                table: "Images",
                newName: "FileName");
        }
    }
}
