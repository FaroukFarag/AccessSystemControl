using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessControlSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_AspNetUsers_OwnerId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_OwnerId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "InviteToken",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Cards");

            migrationBuilder.AddColumn<int>(
                name: "AirfobUserId",
                table: "Visitors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AssignedOwner",
                table: "Units",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AirfobUserId",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_UnitId",
                table: "Cards",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UnitId",
                table: "AspNetUsers",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Units_UnitId",
                table: "AspNetUsers",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Units_UnitId",
                table: "Cards",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Units_UnitId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Units_UnitId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_UnitId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UnitId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AirfobUserId",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "AssignedOwner",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "AirfobUserId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "InviteToken",
                table: "Visitors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Units",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Cards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Cards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Units_OwnerId",
                table: "Units",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_AspNetUsers_OwnerId",
                table: "Units",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
