namespace OnlineDoctorSystem.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddUserIdToSubmissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ContactSubmissions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactSubmissions_UserId",
                table: "ContactSubmissions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactSubmissions_AspNetUsers_UserId",
                table: "ContactSubmissions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactSubmissions_AspNetUsers_UserId",
                table: "ContactSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_ContactSubmissions_UserId",
                table: "ContactSubmissions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ContactSubmissions");
        }
    }
}
