namespace OnlineDoctorSystem.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddFromThirdPartyToDoctor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactEmailFromThirdParty",
                table: "Doctors",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFromThirdParty",
                table: "Doctors",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactEmailFromThirdParty",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "IsFromThirdParty",
                table: "Doctors");
        }
    }
}
