using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessControlSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyAccessGroupUnitRelation : Migration
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
                name: "AccessGroupUnits",
                columns: table => new
                {
                    AccessGroupId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessGroupUnits", x => new { x.AccessGroupId, x.UnitId });
                    table.ForeignKey(
                        name: "FK_AccessGroupUnits_AccessGroups_AccessGroupId",
                        column: x => x.AccessGroupId,
                        principalTable: "AccessGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessGroupUnits_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessGroupUnits_UnitId",
                table: "AccessGroupUnits",
                column: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessGroupUnits");

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
