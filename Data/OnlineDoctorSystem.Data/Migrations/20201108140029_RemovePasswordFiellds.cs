namespace OnlineDoctorSystem.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RemovePasswordFiellds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Doctors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
