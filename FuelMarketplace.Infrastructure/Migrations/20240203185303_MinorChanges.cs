using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelMarketplace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MinorChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferComments_Offers_OfferId",
                table: "OfferComments");

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "PostComments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "OfferComments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferComments_Offers_OfferId",
                table: "OfferComments",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferComments_Offers_OfferId",
                table: "OfferComments");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "PostComments");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "OfferComments");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferComments_Offers_OfferId",
                table: "OfferComments",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id");
        }
    }
}
