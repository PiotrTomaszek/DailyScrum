using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyScrum.Migrations
{
    public partial class adddailymeetingtopost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_DailyMeetings_DailyMeetingId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "DailyMeetingId",
                table: "Posts",
                newName: "MeetingDailyMeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_DailyMeetingId",
                table: "Posts",
                newName: "IX_Posts_MeetingDailyMeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_DailyMeetings_MeetingDailyMeetingId",
                table: "Posts",
                column: "MeetingDailyMeetingId",
                principalTable: "DailyMeetings",
                principalColumn: "DailyMeetingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_DailyMeetings_MeetingDailyMeetingId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "MeetingDailyMeetingId",
                table: "Posts",
                newName: "DailyMeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_MeetingDailyMeetingId",
                table: "Posts",
                newName: "IX_Posts_DailyMeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_DailyMeetings_DailyMeetingId",
                table: "Posts",
                column: "DailyMeetingId",
                principalTable: "DailyMeetings",
                principalColumn: "DailyMeetingId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
