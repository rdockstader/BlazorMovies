using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorMovies.Server.Migrations
{
    public partial class AddAdminRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var SQLQuery = "INSERT INTO AspNetRoles (Id, [Name], NormalizedName) ";
            SQLQuery += "VALUES ('4B9781D0-38C9-4BB0-961D-510E6EC2CC3E', 'Admin', 'Administrator')";
            migrationBuilder.Sql(SQLQuery);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var SQLQuery = "DELETE FROM AspNetRoles ";
            SQLQuery += "WHERE Id = 4B9781D0-38C9-4BB0-961D-510E6EC2CC3E";
            migrationBuilder.Sql(SQLQuery);
        }
    }
}
