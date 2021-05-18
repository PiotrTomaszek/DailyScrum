using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyScrum.Migrations
{
    public partial class addeddailymeetings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_DailyMeetings_DailyMeetingId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ToMeeting",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "DailyMeetingId",
                table: "Messages",
                newName: "TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_DailyMeetingId",
                table: "Messages",
                newName: "IX_Messages_TeamId");

            migrationBuilder.AddColumn<bool>(
                name: "HasFinished",
                table: "DailyMeetings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    DailyPostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FirstQuestion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondQuestion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThirdQuestion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MyProperty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DailyMeetingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.DailyPostId);
                    table.ForeignKey(
                        name: "FK_Posts_AspNetUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_DailyMeetings_DailyMeetingId",
                        column: x => x.DailyMeetingId,
                        principalTable: "DailyMeetings",
                        principalColumn: "DailyMeetingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_DailyMeetingId",
                table: "Posts",
                column: "DailyMeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_FromUserId",
                table: "Posts",
                column: "FromUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Teams_TeamId",
                table: "Messages",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Teams_TeamId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropColumn(
                name: "HasFinished",
                table: "DailyMeetings");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "Messages",
                newName: "DailyMeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_TeamId",
                table: "Messages",
                newName: "IX_Messages_DailyMeetingId");

            migrationBuilder.AddColumn<string>(
                name: "ToMeeting",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_DailyMeetings_DailyMeetingId",
                table: "Messages",
                column: "DailyMeetingId",
                principalTable: "DailyMeetings",
                principalColumn: "DailyMeetingId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
