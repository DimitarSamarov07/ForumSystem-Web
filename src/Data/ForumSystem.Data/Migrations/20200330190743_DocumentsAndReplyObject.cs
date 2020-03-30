using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumSystem.Data.Migrations
{
    public partial class DocumentsAndReplyObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReplies_AspNetUsers_UserId",
                table: "PostReplies");

            migrationBuilder.DropIndex(
                name: "IX_PostReplies_UserId",
                table: "PostReplies");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "PostReplies");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "PostReplies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PostReplies");

            migrationBuilder.AddColumn<int>(
                name: "ReplyId",
                table: "PostReplies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Replies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    UserId1 = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    InnerReplyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Replies_Replies_InnerReplyId",
                        column: x => x.InnerReplyId,
                        principalTable: "Replies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Replies_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostReplies_ReplyId",
                table: "PostReplies",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_InnerReplyId",
                table: "Replies",
                column: "InnerReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_IsDeleted",
                table: "Replies",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_UserId1",
                table: "Replies",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReplies_Replies_ReplyId",
                table: "PostReplies",
                column: "ReplyId",
                principalTable: "Replies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReplies_Replies_ReplyId",
                table: "PostReplies");

            migrationBuilder.DropTable(
                name: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_PostReplies_ReplyId",
                table: "PostReplies");

            migrationBuilder.DropColumn(
                name: "ReplyId",
                table: "PostReplies");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "PostReplies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PostReplies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PostReplies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostReplies_UserId",
                table: "PostReplies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReplies_AspNetUsers_UserId",
                table: "PostReplies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
