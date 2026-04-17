using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GLMS.Migrations
{
    /// <inheritdoc />
    public partial class AddUsdAndZarToServiceRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "ServiceRequests",
                newName: "CostZAR");

            migrationBuilder.AddColumn<decimal>(
                name: "CostUSD",
                table: "ServiceRequests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostUSD",
                table: "ServiceRequests");

            migrationBuilder.RenameColumn(
                name: "CostZAR",
                table: "ServiceRequests",
                newName: "Cost");
        }
    }
}
