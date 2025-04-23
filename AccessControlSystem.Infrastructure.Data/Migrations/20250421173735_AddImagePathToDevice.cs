using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessControlSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Devices");
        }
    }
}
