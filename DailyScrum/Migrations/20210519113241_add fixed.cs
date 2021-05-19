using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyScrum.Migrations
{
    public partial class addfixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Fixed",
                table: "Problems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fixed",
                table: "Problems");
        }
    }
}
