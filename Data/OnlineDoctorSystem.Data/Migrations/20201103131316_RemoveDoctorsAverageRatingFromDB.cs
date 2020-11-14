namespace OnlineDoctorSystem.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RemoveDoctorsAverageRatingFromDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Doctors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Doctors",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
