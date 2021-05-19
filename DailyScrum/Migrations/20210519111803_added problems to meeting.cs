using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyScrum.Migrations
{
    public partial class addedproblemstomeeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_DailyMeetings_DailyMeetingId",
                table: "Problems");

            migrationBuilder.RenameColumn(
                name: "DailyMeetingId",
                table: "Problems",
                newName: "MeetingDailyMeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_Problems_DailyMeetingId",
                table: "Problems",
                newName: "IX_Problems_MeetingDailyMeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_DailyMeetings_MeetingDailyMeetingId",
                table: "Problems",
                column: "MeetingDailyMeetingId",
                principalTable: "DailyMeetings",
                principalColumn: "DailyMeetingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_DailyMeetings_MeetingDailyMeetingId",
                table: "Problems");

            migrationBuilder.RenameColumn(
                name: "MeetingDailyMeetingId",
                table: "Problems",
                newName: "DailyMeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_Problems_MeetingDailyMeetingId",
                table: "Problems",
                newName: "IX_Problems_DailyMeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_DailyMeetings_DailyMeetingId",
                table: "Problems",
                column: "DailyMeetingId",
                principalTable: "DailyMeetings",
                principalColumn: "DailyMeetingId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
