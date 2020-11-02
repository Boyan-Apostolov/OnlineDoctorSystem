using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineDoctorSystem.Data.Migrations
{
    public partial class FixAverageRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Doctors",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Doctors");
        }
    }
}
