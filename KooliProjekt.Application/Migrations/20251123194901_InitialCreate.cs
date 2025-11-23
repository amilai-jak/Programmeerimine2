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
                    AssetClassID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetClasses", x => x.AssetClassID);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyStates",
                columns: table => new
                {
                    StateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UninvestedCash = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deposits = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Withdrawals = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPortfolioValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyStates", x => x.StateID);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    AssetID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetClassID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ticker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRealEstate = table.Column<bool>(type: "bit", nullable: false)
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
                    HoldingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateID = table.Column<int>(type: "int", nullable: false),
                    AssetID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthlyStateStateID = table.Column<int>(type: "int", nullable: true)
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
