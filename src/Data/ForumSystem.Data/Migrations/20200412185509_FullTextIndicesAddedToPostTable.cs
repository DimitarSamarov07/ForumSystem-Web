using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumSystem.Data.Migrations
{
    public partial class FullTextIndicesAddedToPostTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE FULLTEXT CATALOG PostsCatalog
                  GO
                  CREATE FULLTEXT INDEX ON Posts
                   (
                    Title
                       Language 1033,
                    Content
                       Language 1033
                   )
                  KEY INDEX PK_Posts
                  ON
                  PostsCatalog
                  ",
                true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DROP FULLTEXT INDEX ON Posts
                  DROP FULLTEXT CATALOG PostsCatalog",
                true);
        }
    }
}
