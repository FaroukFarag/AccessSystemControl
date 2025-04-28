using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessControlSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionDeviceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Subscriptions_SubscriptionId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_SubscriptionId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "Devices");

            migrationBuilder.CreateTable(
                name: "SubscriptionsDevices",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "int", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionsDevices", x => new { x.SubscriptionId, x.DeviceId });
                    table.ForeignKey(
                        name: "FK_SubscriptionsDevices_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubscriptionsDevices_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionsDevices_DeviceId",
                table: "SubscriptionsDevices",
                column: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionsDevices");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionId",
                table: "Devices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_SubscriptionId",
                table: "Devices",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Subscriptions_SubscriptionId",
                table: "Devices",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id");
        }
    }
}
