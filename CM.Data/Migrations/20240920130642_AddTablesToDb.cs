using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CM.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTablesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ContactsInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ContactsInfo_UserId",
                table: "ContactsInfo",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactsInfo_Users_UserId",
                table: "ContactsInfo",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactsInfo_Users_UserId",
                table: "ContactsInfo");

            migrationBuilder.DropIndex(
                name: "IX_ContactsInfo_UserId",
                table: "ContactsInfo");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ContactsInfo");
        }
    }
}
