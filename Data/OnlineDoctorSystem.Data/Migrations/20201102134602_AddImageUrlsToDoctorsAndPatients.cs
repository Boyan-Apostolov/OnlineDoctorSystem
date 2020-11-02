using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineDoctorSystem.Data.Migrations
{
    public partial class AddImageUrlsToDoctorsAndPatients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Doctors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Patients");
        }
    }
}
