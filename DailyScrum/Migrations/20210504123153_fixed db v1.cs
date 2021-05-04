using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyScrum.Migrations
{
    public partial class fixeddbv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyMeeting_Teams_TeamId",
                table: "DailyMeeting");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_AspNetUsers_FromUserId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_DailyMeeting_DailyMeetingId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Problem_DailyMeeting_DailyMeetingId",
                table: "Problem");

            migrationBuilder.DropForeignKey(
                name: "FK_ScrumTask_AspNetUsers_ExecutorId",
                table: "ScrumTask");

            migrationBuilder.DropForeignKey(
                name: "FK_ScrumTask_DailyMeeting_DailyMeetingId",
                table: "ScrumTask");

            migrationBuilder.DropForeignKey(
                name: "FK_ScrumTask_ScrumTaskBoard_BoardScrumTaskBoardId",
                table: "ScrumTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScrumTaskBoard",
                table: "ScrumTaskBoard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScrumTask",
                table: "ScrumTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Problem",
                table: "Problem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DailyMeeting",
                table: "DailyMeeting");

            migrationBuilder.RenameTable(
                name: "ScrumTaskBoard",
                newName: "ScrumTaskBoards");

            migrationBuilder.RenameTable(
                name: "ScrumTask",
                newName: "ScrumTasks");

            migrationBuilder.RenameTable(
                name: "Problem",
                newName: "Problems");

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "Messages");

            migrationBuilder.RenameTable(
                name: "DailyMeeting",
                newName: "DailyMeetings");

            migrationBuilder.RenameIndex(
                name: "IX_ScrumTask_ExecutorId",
                table: "ScrumTasks",
                newName: "IX_ScrumTasks_ExecutorId");

            migrationBuilder.RenameIndex(
                name: "IX_ScrumTask_DailyMeetingId",
                table: "ScrumTasks",
                newName: "IX_ScrumTasks_DailyMeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_ScrumTask_BoardScrumTaskBoardId",
                table: "ScrumTasks",
                newName: "IX_ScrumTasks_BoardScrumTaskBoardId");

            migrationBuilder.RenameIndex(
                name: "IX_Problem_DailyMeetingId",
                table: "Problems",
                newName: "IX_Problems_DailyMeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_FromUserId",
                table: "Messages",
                newName: "IX_Messages_FromUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_DailyMeetingId",
                table: "Messages",
                newName: "IX_Messages_DailyMeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_DailyMeeting_TeamId",
                table: "DailyMeetings",
                newName: "IX_DailyMeetings_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScrumTaskBoards",
                table: "ScrumTaskBoards",
                column: "ScrumTaskBoardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScrumTasks",
                table: "ScrumTasks",
                column: "ScrumTaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Problems",
                table: "Problems",
                column: "ProblemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "MessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DailyMeetings",
                table: "DailyMeetings",
                column: "DailyMeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyMeetings_Teams_TeamId",
                table: "DailyMeetings",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_FromUserId",
                table: "Messages",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_DailyMeetings_DailyMeetingId",
                table: "Messages",
                column: "DailyMeetingId",
                principalTable: "DailyMeetings",
                principalColumn: "DailyMeetingId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_DailyMeetings_DailyMeetingId",
                table: "Problems",
                column: "DailyMeetingId",
                principalTable: "DailyMeetings",
                principalColumn: "DailyMeetingId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScrumTasks_AspNetUsers_ExecutorId",
                table: "ScrumTasks",
                column: "ExecutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScrumTasks_DailyMeetings_DailyMeetingId",
                table: "ScrumTasks",
                column: "DailyMeetingId",
                principalTable: "DailyMeetings",
                principalColumn: "DailyMeetingId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScrumTasks_ScrumTaskBoards_BoardScrumTaskBoardId",
                table: "ScrumTasks",
                column: "BoardScrumTaskBoardId",
                principalTable: "ScrumTaskBoards",
                principalColumn: "ScrumTaskBoardId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyMeetings_Teams_TeamId",
                table: "DailyMeetings");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_FromUserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_DailyMeetings_DailyMeetingId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Problems_DailyMeetings_DailyMeetingId",
                table: "Problems");

            migrationBuilder.DropForeignKey(
                name: "FK_ScrumTasks_AspNetUsers_ExecutorId",
                table: "ScrumTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ScrumTasks_DailyMeetings_DailyMeetingId",
                table: "ScrumTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ScrumTasks_ScrumTaskBoards_BoardScrumTaskBoardId",
                table: "ScrumTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScrumTasks",
                table: "ScrumTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScrumTaskBoards",
                table: "ScrumTaskBoards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Problems",
                table: "Problems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DailyMeetings",
                table: "DailyMeetings");

            migrationBuilder.RenameTable(
                name: "ScrumTasks",
                newName: "ScrumTask");

            migrationBuilder.RenameTable(
                name: "ScrumTaskBoards",
                newName: "ScrumTaskBoard");

            migrationBuilder.RenameTable(
                name: "Problems",
                newName: "Problem");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "Message");

            migrationBuilder.RenameTable(
                name: "DailyMeetings",
                newName: "DailyMeeting");

            migrationBuilder.RenameIndex(
                name: "IX_ScrumTasks_ExecutorId",
                table: "ScrumTask",
                newName: "IX_ScrumTask_ExecutorId");

            migrationBuilder.RenameIndex(
                name: "IX_ScrumTasks_DailyMeetingId",
                table: "ScrumTask",
                newName: "IX_ScrumTask_DailyMeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_ScrumTasks_BoardScrumTaskBoardId",
                table: "ScrumTask",
                newName: "IX_ScrumTask_BoardScrumTaskBoardId");

            migrationBuilder.RenameIndex(
                name: "IX_Problems_DailyMeetingId",
                table: "Problem",
                newName: "IX_Problem_DailyMeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_FromUserId",
                table: "Message",
                newName: "IX_Message_FromUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_DailyMeetingId",
                table: "Message",
                newName: "IX_Message_DailyMeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_DailyMeetings_TeamId",
                table: "DailyMeeting",
                newName: "IX_DailyMeeting_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScrumTask",
                table: "ScrumTask",
                column: "ScrumTaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScrumTaskBoard",
                table: "ScrumTaskBoard",
                column: "ScrumTaskBoardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Problem",
                table: "Problem",
                column: "ProblemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "MessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DailyMeeting",
                table: "DailyMeeting",
                column: "DailyMeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyMeeting_Teams_TeamId",
                table: "DailyMeeting",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_AspNetUsers_FromUserId",
                table: "Message",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_DailyMeeting_DailyMeetingId",
                table: "Message",
                column: "DailyMeetingId",
                principalTable: "DailyMeeting",
                principalColumn: "DailyMeetingId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Problem_DailyMeeting_DailyMeetingId",
                table: "Problem",
                column: "DailyMeetingId",
                principalTable: "DailyMeeting",
                principalColumn: "DailyMeetingId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScrumTask_AspNetUsers_ExecutorId",
                table: "ScrumTask",
                column: "ExecutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScrumTask_DailyMeeting_DailyMeetingId",
                table: "ScrumTask",
                column: "DailyMeetingId",
                principalTable: "DailyMeeting",
                principalColumn: "DailyMeetingId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScrumTask_ScrumTaskBoard_BoardScrumTaskBoardId",
                table: "ScrumTask",
                column: "BoardScrumTaskBoardId",
                principalTable: "ScrumTaskBoard",
                principalColumn: "ScrumTaskBoardId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
