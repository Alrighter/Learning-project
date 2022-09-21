using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning_project.Migrations
{
    public partial class profilePicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileUserUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileUserUrl",
                table: "AspNetUsers");
        }
    }
}
