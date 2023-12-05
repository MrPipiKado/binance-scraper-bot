using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BinanceBotNuGetPackage.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDeals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DealsVolume",
                table: "Deals",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DealsVolume",
                table: "Deals");
        }
    }
}
