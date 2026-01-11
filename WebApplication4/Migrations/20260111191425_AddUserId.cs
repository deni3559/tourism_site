using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopCart_Users_UserWhoAddTheTourId",
                table: "ShopCart");

            migrationBuilder.RenameColumn(
                name: "UserWhoAddTheTourId",
                table: "ShopCart",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopCart_UserWhoAddTheTourId",
                table: "ShopCart",
                newName: "IX_ShopCart_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopCart_Users_UserId",
                table: "ShopCart",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopCart_Users_UserId",
                table: "ShopCart");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ShopCart",
                newName: "UserWhoAddTheTourId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopCart_UserId",
                table: "ShopCart",
                newName: "IX_ShopCart_UserWhoAddTheTourId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopCart_Users_UserWhoAddTheTourId",
                table: "ShopCart",
                column: "UserWhoAddTheTourId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
