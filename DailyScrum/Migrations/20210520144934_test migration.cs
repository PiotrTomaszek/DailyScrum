using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyScrum.Migrations
{
    public partial class testmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_FromUserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_DailyMeetings_MeetingDailyMeetingId",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "DailyPosts");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_MeetingDailyMeetingId",
                table: "DailyPosts",
                newName: "IX_DailyPosts_MeetingDailyMeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_FromUserId",
                table: "DailyPosts",
                newName: "IX_DailyPosts_FromUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DailyPosts",
                table: "DailyPosts",
                column: "DailyPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyPosts_AspNetUsers_FromUserId",
                table: "DailyPosts",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyPosts_DailyMeetings_MeetingDailyMeetingId",
                table: "DailyPosts",
                column: "MeetingDailyMeetingId",
                principalTable: "DailyMeetings",
                principalColumn: "DailyMeetingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyPosts_AspNetUsers_FromUserId",
                table: "DailyPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyPosts_DailyMeetings_MeetingDailyMeetingId",
                table: "DailyPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DailyPosts",
                table: "DailyPosts");

            migrationBuilder.RenameTable(
                name: "DailyPosts",
                newName: "Posts");

            migrationBuilder.RenameIndex(
                name: "IX_DailyPosts_MeetingDailyMeetingId",
                table: "Posts",
                newName: "IX_Posts_MeetingDailyMeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_DailyPosts_FromUserId",
                table: "Posts",
                newName: "IX_Posts_FromUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "DailyPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_FromUserId",
                table: "Posts",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_DailyMeetings_MeetingDailyMeetingId",
                table: "Posts",
                column: "MeetingDailyMeetingId",
                principalTable: "DailyMeetings",
                principalColumn: "DailyMeetingId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
