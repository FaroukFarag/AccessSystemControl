using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessControlSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionIdToRemainingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Subscriptions_SubscriptionId",
                table: "Cards");

            migrationBuilder.AlterColumn<int>(
                name: "SubscriptionId",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionId",
                table: "AccessGroupUnits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionId",
                table: "AccessGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionId",
                table: "AccessGroupDevices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Subscriptions_SubscriptionId",
                table: "Cards",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Subscriptions_SubscriptionId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "AccessGroupUnits");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "AccessGroups");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "AccessGroupDevices");

            migrationBuilder.AlterColumn<int>(
                name: "SubscriptionId",
                table: "Cards",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Subscriptions_SubscriptionId",
                table: "Cards",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id");
        }
    }
}
