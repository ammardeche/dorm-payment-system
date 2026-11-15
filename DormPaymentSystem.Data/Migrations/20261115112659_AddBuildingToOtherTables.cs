using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DormPaymentSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBuildingToOtherTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "Invitations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "Guests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_BuildingId",
                table: "Reservations",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BuildingId",
                table: "Payments",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_BuildingId",
                table: "Invitations",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_BuildingId",
                table: "Guests",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_Buildings_BuildingId",
                table: "Guests",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Buildings_BuildingId",
                table: "Invitations",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Buildings_BuildingId",
                table: "Payments",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Buildings_BuildingId",
                table: "Reservations",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guests_Buildings_BuildingId",
                table: "Guests");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Buildings_BuildingId",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Buildings_BuildingId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Buildings_BuildingId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_BuildingId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Payments_BuildingId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_BuildingId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Guests_BuildingId",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Guests");
        }
    }
}
