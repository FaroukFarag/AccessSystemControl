using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessControlSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOwnerIdFromAccessGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessGroups_AspNetUsers_OwnerId",
                table: "AccessGroups");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "AccessGroups",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AccessGroups_OwnerId",
                table: "AccessGroups",
                newName: "IX_AccessGroups_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessGroups_AspNetUsers_UserId",
                table: "AccessGroups",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessGroups_AspNetUsers_UserId",
                table: "AccessGroups");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AccessGroups",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_AccessGroups_UserId",
                table: "AccessGroups",
                newName: "IX_AccessGroups_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessGroups_AspNetUsers_OwnerId",
                table: "AccessGroups",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
