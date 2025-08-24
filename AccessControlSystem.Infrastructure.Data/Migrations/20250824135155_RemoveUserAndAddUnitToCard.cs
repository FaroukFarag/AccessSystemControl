using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessControlSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserAndAddUnitToCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_AspNetUsers_OwnerId",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Cards",
                newName: "UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_OwnerId",
                table: "Cards",
                newName: "IX_Cards_UnitId");

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
                name: "FK_Cards_Units_UnitId",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "UnitId",
                table: "Cards",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_UnitId",
                table: "Cards",
                newName: "IX_Cards_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_AspNetUsers_OwnerId",
                table: "Cards",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
