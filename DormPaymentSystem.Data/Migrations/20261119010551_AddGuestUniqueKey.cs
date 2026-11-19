using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DormPaymentSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGuestUniqueKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guests_Buildings_BuildingId",
                table: "Guests");

            migrationBuilder.DropIndex(
                name: "IX_Guests_BuildingId",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Guests");

            migrationBuilder.AlterColumn<string>(
                name: "NationalId",
                table: "Guests",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_NationalId",
                table: "Guests",
                column: "NationalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Guests_NationalId",
                table: "Guests");

            migrationBuilder.AlterColumn<string>(
                name: "NationalId",
                table: "Guests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "Guests",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
        }
    }
}
