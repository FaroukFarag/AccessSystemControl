using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessControlSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeSubscriptionOptionalInUserAndAddOwnersAndAccessGroupsToUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessGroupDevices_Units_UnitId",
                table: "AccessGroupDevices");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_AspNetUsers_UserId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_UserId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_AccessGroupDevices_UnitId",
                table: "AccessGroupDevices");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "AccessGroupDevices");

            migrationBuilder.AlterColumn<int>(
                name: "SubscriptionId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "AccessGroups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UnitId",
                table: "AspNetUsers",
                column: "UnitId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Units_UnitId",
                table: "AspNetUsers",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessGroups_Units_UnitId",
                table: "AccessGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Units_UnitId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UnitId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AccessGroups_UnitId",
                table: "AccessGroups");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "AccessGroups");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SubscriptionId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "AccessGroupDevices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_UserId",
                table: "Units",
                column: "UserId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Units_AspNetUsers_UserId",
                table: "Units",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
