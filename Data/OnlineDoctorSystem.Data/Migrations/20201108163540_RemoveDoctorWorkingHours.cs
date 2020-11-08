using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineDoctorSystem.Data.Migrations
{
    public partial class RemoveDoctorWorkingHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingHours",
                table: "Doctors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkingHours",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
