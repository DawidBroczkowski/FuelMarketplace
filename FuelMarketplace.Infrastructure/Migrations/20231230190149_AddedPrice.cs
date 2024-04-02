using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelMarketplace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_PostComment_PostCommentId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferComment_Offers_OfferId",
                table: "OfferComment");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferComment_Users_UserId",
                table: "OfferComment");

            migrationBuilder.DropForeignKey(
                name: "FK_PostComment_Posts_PostId",
                table: "PostComment");

            migrationBuilder.DropForeignKey(
                name: "FK_PostComment_Users_UserId",
                table: "PostComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostComment",
                table: "PostComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferComment",
                table: "OfferComment");

            migrationBuilder.RenameTable(
                name: "PostComment",
                newName: "PostComments");

            migrationBuilder.RenameTable(
                name: "OfferComment",
                newName: "OfferComments");

            migrationBuilder.RenameIndex(
                name: "IX_PostComment_UserId",
                table: "PostComments",
                newName: "IX_PostComments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PostComment_PostId",
                table: "PostComments",
                newName: "IX_PostComments_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_OfferComment_UserId",
                table: "OfferComments",
                newName: "IX_OfferComments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OfferComment_OfferId",
                table: "OfferComments",
                newName: "IX_OfferComments_OfferId");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Offers",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "PostComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "OfferComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostComments",
                table: "PostComments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferComments",
                table: "OfferComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_PostComments_PostCommentId",
                table: "Images",
                column: "PostCommentId",
                principalTable: "PostComments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferComments_Offers_OfferId",
                table: "OfferComments",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferComments_Users_UserId",
                table: "OfferComments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_Posts_PostId",
                table: "PostComments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_Users_UserId",
                table: "PostComments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_PostComments_PostCommentId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferComments_Offers_OfferId",
                table: "OfferComments");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferComments_Users_UserId",
                table: "OfferComments");

            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_Posts_PostId",
                table: "PostComments");

            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_Users_UserId",
                table: "PostComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostComments",
                table: "PostComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferComments",
                table: "OfferComments");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "PostComments");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "OfferComments");

            migrationBuilder.RenameTable(
                name: "PostComments",
                newName: "PostComment");

            migrationBuilder.RenameTable(
                name: "OfferComments",
                newName: "OfferComment");

            migrationBuilder.RenameIndex(
                name: "IX_PostComments_UserId",
                table: "PostComment",
                newName: "IX_PostComment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PostComments_PostId",
                table: "PostComment",
                newName: "IX_PostComment_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_OfferComments_UserId",
                table: "OfferComment",
                newName: "IX_OfferComment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OfferComments_OfferId",
                table: "OfferComment",
                newName: "IX_OfferComment_OfferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostComment",
                table: "PostComment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferComment",
                table: "OfferComment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_PostComment_PostCommentId",
                table: "Images",
                column: "PostCommentId",
                principalTable: "PostComment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferComment_Offers_OfferId",
                table: "OfferComment",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferComment_Users_UserId",
                table: "OfferComment",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_Posts_PostId",
                table: "PostComment",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_Users_UserId",
                table: "PostComment",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
