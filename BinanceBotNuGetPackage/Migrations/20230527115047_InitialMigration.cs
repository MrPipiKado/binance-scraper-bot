using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BinanceBotNuGetPackage.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DateIntervals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Period = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateIntervals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryCryptoInfo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CryptoName = table.Column<string>(type: "TEXT", nullable: false),
                    ShortCryptoName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryCryptoInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Deals",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstInTradingPair = table.Column<int>(type: "INTEGER", nullable: false),
                    SecondInTradingPair = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentPrice = table.Column<double>(type: "REAL", nullable: false),
                    HighestPrice = table.Column<double>(type: "REAL", nullable: false),
                    LowestPrice = table.Column<double>(type: "REAL", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Period = table.Column<int>(type: "INTEGER", nullable: false),
                    ChangePercentage = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deals", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Deals_DateIntervals_Period",
                        column: x => x.Period,
                        principalTable: "DateIntervals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deals_PrimaryCryptoInfo_FirstInTradingPair",
                        column: x => x.FirstInTradingPair,
                        principalTable: "PrimaryCryptoInfo",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deals_PrimaryCryptoInfo_SecondInTradingPair",
                        column: x => x.SecondInTradingPair,
                        principalTable: "PrimaryCryptoInfo",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deals_FirstInTradingPair",
                table: "Deals",
                column: "FirstInTradingPair");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_Period",
                table: "Deals",
                column: "Period");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_SecondInTradingPair",
                table: "Deals",
                column: "SecondInTradingPair");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deals");

            migrationBuilder.DropTable(
                name: "DateIntervals");

            migrationBuilder.DropTable(
                name: "PrimaryCryptoInfo");
        }
    }
}
