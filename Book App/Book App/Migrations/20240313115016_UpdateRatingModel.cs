using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book_App.Migrations
{
    public partial class UpdateRatingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Ratings");

            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Books",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Ratings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
