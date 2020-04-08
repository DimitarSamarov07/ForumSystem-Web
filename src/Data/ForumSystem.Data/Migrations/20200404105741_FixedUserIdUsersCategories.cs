using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumSystem.Data.Migrations
{
    public partial class FixedUserIdUsersCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersCategories_AspNetUsers_UserId1",
                table: "UsersCategories");

            migrationBuilder.DropIndex(
                name: "IX_UsersCategories_UserId1",
                table: "UsersCategories");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UsersCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersCategories",
                table: "UsersCategories");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UsersCategories",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                table: "UsersCategories",
                columns: new[] { "UserId", "CategoryId" },
                name: "PK_UsersCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCategories_AspNetUsers_UserId",
                table: "UsersCategories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersCategories_AspNetUsers_UserId",
                table: "UsersCategories");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UsersCategories",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "UsersCategories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersCategories_UserId1",
                table: "UsersCategories",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCategories_AspNetUsers_UserId1",
                table: "UsersCategories",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
