using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KooliProjekt.Application.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetClasses",
                columns: table => new
                {
                    AssetClassID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetClasses", x => x.AssetClassID);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyStates",
                columns: table => new
                {
                    StateID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UninvestedCash = table.Column<decimal>(type: "TEXT", nullable: false),
                    Deposits = table.Column<decimal>(type: "TEXT", nullable: false),
                    Withdrawals = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalPortfolioValue = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyStates", x => x.StateID);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    AssetID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssetClassID = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Ticker = table.Column<string>(type: "TEXT", nullable: true),
                    IsRealEstate = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.AssetID);
                    table.ForeignKey(
                        name: "FK_Assets_AssetClasses_AssetClassID",
                        column: x => x.AssetClassID,
                        principalTable: "AssetClasses",
                        principalColumn: "AssetClassID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyHoldings",
                columns: table => new
                {
                    HoldingID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StateID = table.Column<int>(type: "INTEGER", nullable: false),
                    AssetID = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    Value = table.Column<decimal>(type: "TEXT", nullable: false),
                    MonthlyStateStateID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyHoldings", x => x.HoldingID);
                    table.ForeignKey(
                        name: "FK_MonthlyHoldings_Assets_AssetID",
                        column: x => x.AssetID,
                        principalTable: "Assets",
                        principalColumn: "AssetID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyHoldings_MonthlyStates_MonthlyStateStateID",
                        column: x => x.MonthlyStateStateID,
                        principalTable: "MonthlyStates",
                        principalColumn: "StateID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetClassID",
                table: "Assets",
                column: "AssetClassID");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyHoldings_AssetID",
                table: "MonthlyHoldings",
                column: "AssetID");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyHoldings_MonthlyStateStateID",
                table: "MonthlyHoldings",
                column: "MonthlyStateStateID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlyHoldings");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "MonthlyStates");

            migrationBuilder.DropTable(
                name: "AssetClasses");
        }
    }
}
