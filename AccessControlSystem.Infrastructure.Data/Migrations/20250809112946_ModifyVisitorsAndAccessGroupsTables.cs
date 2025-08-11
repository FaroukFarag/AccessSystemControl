using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessControlSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyVisitorsAndAccessGroupsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visitors_Units_UnitId",
                table: "Visitors");

            migrationBuilder.DropIndex(
                name: "IX_Visitors_UnitId",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Visitors");

            migrationBuilder.RenameColumn(
                name: "VisitTo",
                table: "Visitors",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "VisitFrom",
                table: "Visitors",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Visitors",
                newName: "Mobile");

            migrationBuilder.AddColumn<string>(
                name: "InviteToken",
                table: "Visitors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "AirfobAccessLevelId",
                table: "AccessGroups",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AccessGroupVisitor",
                columns: table => new
                {
                    AccessGroupsId = table.Column<int>(type: "int", nullable: false),
                    VisitorsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessGroupVisitor", x => new { x.AccessGroupsId, x.VisitorsId });
                    table.ForeignKey(
                        name: "FK_AccessGroupVisitor_AccessGroups_AccessGroupsId",
                        column: x => x.AccessGroupsId,
                        principalTable: "AccessGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessGroupVisitor_Visitors_VisitorsId",
                        column: x => x.VisitorsId,
                        principalTable: "Visitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessGroupVisitor_VisitorsId",
                table: "AccessGroupVisitor",
                column: "VisitorsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessGroupVisitor");

            migrationBuilder.DropColumn(
                name: "InviteToken",
                table: "Visitors");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Visitors",
                newName: "VisitTo");

            migrationBuilder.RenameColumn(
                name: "Mobile",
                table: "Visitors",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Visitors",
                newName: "VisitFrom");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Visitors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AirfobAccessLevelId",
                table: "AccessGroups",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_UnitId",
                table: "Visitors",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitors_Units_UnitId",
                table: "Visitors",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
