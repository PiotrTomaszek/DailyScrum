using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyScrum.Migrations
{
    public partial class addedteams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamMemberTeamId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TeamMemberTeamId",
                table: "AspNetUsers",
                column: "TeamMemberTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Team_TeamMemberTeamId",
                table: "AspNetUsers",
                column: "TeamMemberTeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Team_TeamMemberTeamId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TeamMemberTeamId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TeamMemberTeamId",
                table: "AspNetUsers");
        }
    }
}
