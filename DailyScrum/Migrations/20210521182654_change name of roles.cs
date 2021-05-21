using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyScrum.Migrations
{
    public partial class changenameofroles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ScrumRoles_TeamRoleRoleId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "ScrumRoles",
                newName: "ScrumRoleId");

            migrationBuilder.RenameColumn(
                name: "TeamRoleRoleId",
                table: "AspNetUsers",
                newName: "TeamRoleScrumRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_TeamRoleRoleId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_TeamRoleScrumRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ScrumRoles_TeamRoleScrumRoleId",
                table: "AspNetUsers",
                column: "TeamRoleScrumRoleId",
                principalTable: "ScrumRoles",
                principalColumn: "ScrumRoleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ScrumRoles_TeamRoleScrumRoleId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ScrumRoleId",
                table: "ScrumRoles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "TeamRoleScrumRoleId",
                table: "AspNetUsers",
                newName: "TeamRoleRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_TeamRoleScrumRoleId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_TeamRoleRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ScrumRoles_TeamRoleRoleId",
                table: "AspNetUsers",
                column: "TeamRoleRoleId",
                principalTable: "ScrumRoles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
