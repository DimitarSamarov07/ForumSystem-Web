using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumSystem.Data.Migrations
{
    public partial class RemovedUselessMappingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReplyReports_PostReplies_PostReplyId",
                table: "PostReplyReports");

            migrationBuilder.DropTable(
                name: "PostReplies");

            migrationBuilder.DropIndex(
                name: "IX_PostReplyReports_PostReplyId",
                table: "PostReplyReports");

            migrationBuilder.DropColumn(
                name: "PostReplyId",
                table: "PostReplyReports");

            migrationBuilder.AddColumn<int>(
                name: "ReplyId",
                table: "PostReplyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PostReplyReports_ReplyId",
                table: "PostReplyReports",
                column: "ReplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReplyReports_Replies_ReplyId",
                table: "PostReplyReports",
                column: "ReplyId",
                principalTable: "Replies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReplyReports_Replies_ReplyId",
                table: "PostReplyReports");

            migrationBuilder.DropIndex(
                name: "IX_PostReplyReports_ReplyId",
                table: "PostReplyReports");

            migrationBuilder.DropColumn(
                name: "ReplyId",
                table: "PostReplyReports");

            migrationBuilder.AddColumn<int>(
                name: "PostReplyId",
                table: "PostReplyReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PostReplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    ReplyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostReplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostReplies_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostReplies_Replies_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "Replies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostReplyReports_PostReplyId",
                table: "PostReplyReports",
                column: "PostReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_PostReplies_IsDeleted",
                table: "PostReplies",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PostReplies_PostId",
                table: "PostReplies",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostReplies_ReplyId",
                table: "PostReplies",
                column: "ReplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReplyReports_PostReplies_PostReplyId",
                table: "PostReplyReports",
                column: "PostReplyId",
                principalTable: "PostReplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
