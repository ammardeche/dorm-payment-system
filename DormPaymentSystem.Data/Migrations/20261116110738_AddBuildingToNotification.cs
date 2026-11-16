using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DormPaymentSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBuildingToNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_BuildingId",
                table: "Notifications",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Buildings_BuildingId",
                table: "Notifications",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Buildings_BuildingId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_BuildingId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Notifications");
        }
    }
}
