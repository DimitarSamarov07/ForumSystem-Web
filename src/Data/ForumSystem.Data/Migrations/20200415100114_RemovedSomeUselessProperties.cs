using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumSystem.Data.Migrations
{
    public partial class RemovedSomeUselessProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Replies_InnerReplyId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_InnerReplyId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "InnerReplyId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "Downvotes",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Upvotes",
                table: "Posts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InnerReplyId",
                table: "Replies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Replies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Downvotes",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Upvotes",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Replies_InnerReplyId",
                table: "Replies",
                column: "InnerReplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Replies_InnerReplyId",
                table: "Replies",
                column: "InnerReplyId",
                principalTable: "Replies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
