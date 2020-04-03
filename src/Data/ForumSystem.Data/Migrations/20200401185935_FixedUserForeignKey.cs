using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumSystem.Data.Migrations
{
    public partial class FixedUserForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_UserId1",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_AspNetUsers_UserId1",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_UserId1",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId1",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Replies",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Replies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_PostId",
                table: "Replies",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_UserId",
                table: "Replies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Posts_PostId",
                table: "Replies",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_AspNetUsers_UserId",
                table: "Replies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Posts_PostId",
                table: "Replies");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_AspNetUsers_UserId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_PostId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_UserId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Replies");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Replies",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Replies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Posts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Replies_UserId1",
                table: "Replies",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId1",
                table: "Posts",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_UserId1",
                table: "Posts",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_AspNetUsers_UserId1",
                table: "Replies",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
