using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineDoctorSystem.Data.Migrations
{
    public partial class AddIsReviewedToConsultations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReviewed",
                table: "Consultations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReviewed",
                table: "Consultations");
        }
    }
}
