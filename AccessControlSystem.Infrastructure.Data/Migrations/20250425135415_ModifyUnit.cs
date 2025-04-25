using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessControlSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CardNumber",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "AccessGroupDevices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessGroupDevices_UnitId",
                table: "AccessGroupDevices",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessGroupDevices_Units_UnitId",
                table: "AccessGroupDevices",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessGroupDevices_Units_UnitId",
                table: "AccessGroupDevices");

            migrationBuilder.DropIndex(
                name: "IX_AccessGroupDevices_UnitId",
                table: "AccessGroupDevices");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "AccessGroupDevices");
        }
    }
}
