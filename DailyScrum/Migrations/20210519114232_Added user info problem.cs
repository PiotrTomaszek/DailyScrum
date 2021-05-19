using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyScrum.Migrations
{
    public partial class Addeduserinfoproblem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromUserId",
                table: "Problems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Problems_FromUserId",
                table: "Problems",
                column: "FromUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_AspNetUsers_FromUserId",
                table: "Problems",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_AspNetUsers_FromUserId",
                table: "Problems");

            migrationBuilder.DropIndex(
                name: "IX_Problems_FromUserId",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "FromUserId",
                table: "Problems");
        }
    }
}
