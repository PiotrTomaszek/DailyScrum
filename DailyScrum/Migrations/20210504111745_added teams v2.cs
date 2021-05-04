using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyScrum.Migrations
{
    public partial class addedteamsv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Team_TeamMemberTeamId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Team",
                table: "Team");

            migrationBuilder.RenameTable(
                name: "Team",
                newName: "Teams");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teams",
                table: "Teams",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Teams_TeamMemberTeamId",
                table: "AspNetUsers",
                column: "TeamMemberTeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Teams_TeamMemberTeamId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teams",
                table: "Teams");

            migrationBuilder.RenameTable(
                name: "Teams",
                newName: "Team");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Team",
                table: "Team",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Team_TeamMemberTeamId",
                table: "AspNetUsers",
                column: "TeamMemberTeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
