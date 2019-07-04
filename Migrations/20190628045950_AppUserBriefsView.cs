using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenSongWeb.Migrations
{
    public partial class AppUserBriefsView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW View_AppUserBriefs AS 
        SELECT u.Id, u.DisplayName, Count(s.Id) as SongCount 
        FROM AspNetUsers u 
        JOIN OSSongs s on u.Id = s.CreatedById 
        GROUP BY u.Id, U.DisplayName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW View_AppUserBriefs");
        }
    }
}
