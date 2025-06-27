using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessControlSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitorsAndManyToManyUnitAccessGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessGroups_Units_UnitId",
                table: "AccessGroups");

            migrationBuilder.DropIndex(
                name: "IX_AccessGroups_UnitId",
                table: "AccessGroups");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "AccessGroups");

            migrationBuilder.CreateTable(
                name: "UnitAccessGroups",
                columns: table => new
                {
                    AccessGroupId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitAccessGroups", x => new { x.AccessGroupId, x.UnitId });
                    table.ForeignKey(
                        name: "FK_UnitAccessGroups_AccessGroups_AccessGroupId",
                        column: x => x.AccessGroupId,
                        principalTable: "AccessGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitAccessGroups_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VisitFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VisitTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visitors_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Visitors_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitAccessGroups_UnitId",
                table: "UnitAccessGroups",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_SubscriptionId",
                table: "Visitors",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_UnitId",
                table: "Visitors",
                column: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnitAccessGroups");

            migrationBuilder.DropTable(
                name: "Visitors");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "AccessGroups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessGroups_UnitId",
                table: "AccessGroups",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessGroups_Units_UnitId",
                table: "AccessGroups",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }
    }
}
