using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyScrum.Migrations
{
    public partial class fixeddb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyMeeting",
                columns: table => new
                {
                    DailyMeetingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMeeting", x => x.DailyMeetingId);
                    table.ForeignKey(
                        name: "FK_DailyMeeting_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScrumTaskBoard",
                columns: table => new
                {
                    ScrumTaskBoardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrumTaskBoard", x => x.ScrumTaskBoardId);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ToMeeting = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DailyMeetingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Message_AspNetUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_DailyMeeting_DailyMeetingId",
                        column: x => x.DailyMeetingId,
                        principalTable: "DailyMeeting",
                        principalColumn: "DailyMeetingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Problem",
                columns: table => new
                {
                    ProblemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DailyMeetingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Problem", x => x.ProblemId);
                    table.ForeignKey(
                        name: "FK_Problem_DailyMeeting_DailyMeetingId",
                        column: x => x.DailyMeetingId,
                        principalTable: "DailyMeeting",
                        principalColumn: "DailyMeetingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScrumTask",
                columns: table => new
                {
                    ScrumTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    ExecutorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BoardScrumTaskBoardId = table.Column<int>(type: "int", nullable: true),
                    DailyMeetingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrumTask", x => x.ScrumTaskId);
                    table.ForeignKey(
                        name: "FK_ScrumTask_AspNetUsers_ExecutorId",
                        column: x => x.ExecutorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScrumTask_DailyMeeting_DailyMeetingId",
                        column: x => x.DailyMeetingId,
                        principalTable: "DailyMeeting",
                        principalColumn: "DailyMeetingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScrumTask_ScrumTaskBoard_BoardScrumTaskBoardId",
                        column: x => x.BoardScrumTaskBoardId,
                        principalTable: "ScrumTaskBoard",
                        principalColumn: "ScrumTaskBoardId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyMeeting_TeamId",
                table: "DailyMeeting",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_DailyMeetingId",
                table: "Message",
                column: "DailyMeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_FromUserId",
                table: "Message",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Problem_DailyMeetingId",
                table: "Problem",
                column: "DailyMeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_ScrumTask_BoardScrumTaskBoardId",
                table: "ScrumTask",
                column: "BoardScrumTaskBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_ScrumTask_DailyMeetingId",
                table: "ScrumTask",
                column: "DailyMeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_ScrumTask_ExecutorId",
                table: "ScrumTask",
                column: "ExecutorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Problem");

            migrationBuilder.DropTable(
                name: "ScrumTask");

            migrationBuilder.DropTable(
                name: "DailyMeeting");

            migrationBuilder.DropTable(
                name: "ScrumTaskBoard");
        }
    }
}
