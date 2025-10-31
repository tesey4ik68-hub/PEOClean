using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PEOcleanWPFApp.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeAssignmentAndRenameRateFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MonthlyRateJanitor",
                table: "ServiceAddresses",
                newName: "JanitorRate");

            migrationBuilder.RenameColumn(
                name: "MonthlyRateCleaner",
                table: "ServiceAddresses",
                newName: "CleanerRate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JanitorRate",
                table: "ServiceAddresses",
                newName: "MonthlyRateJanitor");

            migrationBuilder.RenameColumn(
                name: "CleanerRate",
                table: "ServiceAddresses",
                newName: "MonthlyRateCleaner");
        }
    }
}
