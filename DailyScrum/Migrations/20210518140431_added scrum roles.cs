using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyScrum.Migrations
{
    public partial class addedscrumroles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Role_TeamRoleRoleId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "ScrumRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScrumRoles",
                table: "ScrumRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ScrumRoles_TeamRoleRoleId",
                table: "AspNetUsers",
                column: "TeamRoleRoleId",
                principalTable: "ScrumRoles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ScrumRoles_TeamRoleRoleId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScrumRoles",
                table: "ScrumRoles");

            migrationBuilder.RenameTable(
                name: "ScrumRoles",
                newName: "Role");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Role_TeamRoleRoleId",
                table: "AspNetUsers",
                column: "TeamRoleRoleId",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
