using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyScrum.Migrations
{
    public partial class addingroletousers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScrumTasks");

            migrationBuilder.DropTable(
                name: "ScrumTaskBoards");

            migrationBuilder.AddColumn<int>(
                name: "TeamRoleRoleId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TeamRoleRoleId",
                table: "AspNetUsers",
                column: "TeamRoleRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Role_TeamRoleRoleId",
                table: "AspNetUsers",
                column: "TeamRoleRoleId",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Role_TeamRoleRoleId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TeamRoleRoleId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TeamRoleRoleId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ScrumTaskBoards",
                columns: table => new
                {
                    ScrumTaskBoardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrumTaskBoards", x => x.ScrumTaskBoardId);
                });

            migrationBuilder.CreateTable(
                name: "ScrumTasks",
                columns: table => new
                {
                    ScrumTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoardScrumTaskBoardId = table.Column<int>(type: "int", nullable: true),
                    DailyMeetingId = table.Column<int>(type: "int", nullable: true),
                    ExecutorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrumTasks", x => x.ScrumTaskId);
                    table.ForeignKey(
                        name: "FK_ScrumTasks_AspNetUsers_ExecutorId",
                        column: x => x.ExecutorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScrumTasks_DailyMeetings_DailyMeetingId",
                        column: x => x.DailyMeetingId,
                        principalTable: "DailyMeetings",
                        principalColumn: "DailyMeetingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScrumTasks_ScrumTaskBoards_BoardScrumTaskBoardId",
                        column: x => x.BoardScrumTaskBoardId,
                        principalTable: "ScrumTaskBoards",
                        principalColumn: "ScrumTaskBoardId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScrumTasks_BoardScrumTaskBoardId",
                table: "ScrumTasks",
                column: "BoardScrumTaskBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_ScrumTasks_DailyMeetingId",
                table: "ScrumTasks",
                column: "DailyMeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_ScrumTasks_ExecutorId",
                table: "ScrumTasks",
                column: "ExecutorId");
        }
    }
}
