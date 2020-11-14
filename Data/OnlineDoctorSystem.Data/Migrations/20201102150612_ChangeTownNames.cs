namespace OnlineDoctorSystem.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ChangeTownNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TownName",
                table: "Towns");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Towns",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Towns");

            migrationBuilder.AddColumn<string>(
                name: "TownName",
                table: "Towns",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
