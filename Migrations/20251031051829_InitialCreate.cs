using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PEOcleanWPFApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Floors = table.Column<int>(type: "INTEGER", nullable: false),
                    Entrances = table.Column<int>(type: "INTEGER", nullable: false),
                    YardArea = table.Column<decimal>(type: "TEXT", nullable: true),
                    MonthlyRate = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ReplacementEmployeeId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasPhoto = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_Employees_ReplacementEmployeeId",
                        column: x => x.ReplacementEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsPaid = table.Column<bool>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeServiceAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceAddressId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeServiceAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeServiceAddresses_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeServiceAddresses_ServiceAddresses_ServiceAddressId",
                        column: x => x.ServiceAddressId,
                        principalTable: "ServiceAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HouseWorkSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServiceAddressId = table.Column<int>(type: "INTEGER", nullable: false),
                    WorkTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    DayOfWeek = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseWorkSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouseWorkSchedules_ServiceAddresses_ServiceAddressId",
                        column: x => x.ServiceAddressId,
                        principalTable: "ServiceAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouseWorkSchedules_WorkTypes_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalTable: "WorkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceAddressId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WorkTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    PhotoPath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkReports_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkReports_ServiceAddresses_ServiceAddressId",
                        column: x => x.ServiceAddressId,
                        principalTable: "ServiceAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkReports_WorkTypes_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalTable: "WorkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceRecordServiceAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AttendanceRecordId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceAddressId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRecordServiceAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceRecordServiceAddresses_AttendanceRecords_AttendanceRecordId",
                        column: x => x.AttendanceRecordId,
                        principalTable: "AttendanceRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceRecordServiceAddresses_ServiceAddresses_ServiceAddressId",
                        column: x => x.ServiceAddressId,
                        principalTable: "ServiceAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_EmployeeId",
                table: "AttendanceRecords",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_ReplacementEmployeeId",
                table: "AttendanceRecords",
                column: "ReplacementEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecordServiceAddresses_AttendanceRecordId",
                table: "AttendanceRecordServiceAddresses",
                column: "AttendanceRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecordServiceAddresses_ServiceAddressId",
                table: "AttendanceRecordServiceAddresses",
                column: "ServiceAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeServiceAddresses_EmployeeId",
                table: "EmployeeServiceAddresses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeServiceAddresses_ServiceAddressId",
                table: "EmployeeServiceAddresses",
                column: "ServiceAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseWorkSchedules_ServiceAddressId",
                table: "HouseWorkSchedules",
                column: "ServiceAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseWorkSchedules_WorkTypeId",
                table: "HouseWorkSchedules",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_EmployeeId",
                table: "Payments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkReports_EmployeeId",
                table: "WorkReports",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkReports_ServiceAddressId",
                table: "WorkReports",
                column: "ServiceAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkReports_WorkTypeId",
                table: "WorkReports",
                column: "WorkTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceRecordServiceAddresses");

            migrationBuilder.DropTable(
                name: "EmployeeServiceAddresses");

            migrationBuilder.DropTable(
                name: "HouseWorkSchedules");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "WorkReports");

            migrationBuilder.DropTable(
                name: "AttendanceRecords");

            migrationBuilder.DropTable(
                name: "ServiceAddresses");

            migrationBuilder.DropTable(
                name: "WorkTypes");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
