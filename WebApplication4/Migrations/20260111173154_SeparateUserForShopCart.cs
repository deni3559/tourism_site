using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPortal.Migrations
{
    /// <inheritdoc />
    public partial class SeparateUserForShopCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserWhoAddTheTourId",
                table: "ShopCart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShopCart_UserWhoAddTheTourId",
                table: "ShopCart",
                column: "UserWhoAddTheTourId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopCart_Users_UserWhoAddTheTourId",
                table: "ShopCart",
                column: "UserWhoAddTheTourId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopCart_Users_UserWhoAddTheTourId",
                table: "ShopCart");

            migrationBuilder.DropIndex(
                name: "IX_ShopCart_UserWhoAddTheTourId",
                table: "ShopCart");

            migrationBuilder.DropColumn(
                name: "UserWhoAddTheTourId",
                table: "ShopCart");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
