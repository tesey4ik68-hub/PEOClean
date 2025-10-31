using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PEOcleanWPFApp.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeAssignmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkReports_AttendanceRecords_AttendanceRecordId",
                table: "WorkReports");

            migrationBuilder.CreateTable(
                name: "EmployeeAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceAddressId = table.Column<int>(type: "INTEGER", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsPrimary = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAssignments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeAssignments_ServiceAddresses_ServiceAddressId",
                        column: x => x.ServiceAddressId,
                        principalTable: "ServiceAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAssignments_EmployeeId",
                table: "EmployeeAssignments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAssignments_ServiceAddressId",
                table: "EmployeeAssignments",
                column: "ServiceAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkReports_AttendanceRecords_AttendanceRecordId",
                table: "WorkReports",
                column: "AttendanceRecordId",
                principalTable: "AttendanceRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkReports_AttendanceRecords_AttendanceRecordId",
                table: "WorkReports");

            migrationBuilder.DropTable(
                name: "EmployeeAssignments");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkReports_AttendanceRecords_AttendanceRecordId",
                table: "WorkReports",
                column: "AttendanceRecordId",
                principalTable: "AttendanceRecords",
                principalColumn: "Id");
        }
    }
}
