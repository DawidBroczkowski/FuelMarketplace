using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelMarketplace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPostCommentsFromOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComment_Offers_OfferId",
                table: "PostComment");

            migrationBuilder.DropIndex(
                name: "IX_PostComment_OfferId",
                table: "PostComment");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "PostComment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OfferId",
                table: "PostComment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostComment_OfferId",
                table: "PostComment",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_Offers_OfferId",
                table: "PostComment",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id");
        }
    }
}
