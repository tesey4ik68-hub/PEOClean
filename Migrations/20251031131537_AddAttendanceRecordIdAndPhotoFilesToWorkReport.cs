using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PEOcleanWPFApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendanceRecordIdAndPhotoFilesToWorkReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttendanceRecordId",
                table: "WorkReports",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "WorkReports",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhotoFiles",
                table: "WorkReports",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_WorkReports_AttendanceRecordId",
                table: "WorkReports",
                column: "AttendanceRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkReports_AttendanceRecords_AttendanceRecordId",
                table: "WorkReports",
                column: "AttendanceRecordId",
                principalTable: "AttendanceRecords",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkReports_AttendanceRecords_AttendanceRecordId",
                table: "WorkReports");

            migrationBuilder.DropIndex(
                name: "IX_WorkReports_AttendanceRecordId",
                table: "WorkReports");

            migrationBuilder.DropColumn(
                name: "AttendanceRecordId",
                table: "WorkReports");

            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "WorkReports");

            migrationBuilder.DropColumn(
                name: "PhotoFiles",
                table: "WorkReports");
        }
    }
}
