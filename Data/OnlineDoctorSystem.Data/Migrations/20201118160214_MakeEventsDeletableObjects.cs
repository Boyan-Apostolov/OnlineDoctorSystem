namespace OnlineDoctorSystem.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class MakeEventsDeletableObjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Events",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Events",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CalendarEventId",
                table: "Consultations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_IsDeleted",
                table: "Events",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_CalendarEventId",
                table: "Consultations",
                column: "CalendarEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_Events_CalendarEventId",
                table: "Consultations",
                column: "CalendarEventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_Events_CalendarEventId",
                table: "Consultations");

            migrationBuilder.DropIndex(
                name: "IX_Events_IsDeleted",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Consultations_CalendarEventId",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CalendarEventId",
                table: "Consultations");
        }
    }
}
