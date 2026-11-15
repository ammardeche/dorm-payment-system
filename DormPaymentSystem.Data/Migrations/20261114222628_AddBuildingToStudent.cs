using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DormPaymentSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBuildingToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Students_BuildingId",
                table: "Students",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Buildings_BuildingId",
                table: "Students",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Buildings_BuildingId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_BuildingId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Students");
        }
    }
}
