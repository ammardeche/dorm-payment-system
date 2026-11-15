using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DormPaymentSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMangerRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7c1d9e52-3b8a-4f60-a2d4-5e6f7a8b9c0d", null, "Manager", "MANAGER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c1d9e52-3b8a-4f60-a2d4-5e6f7a8b9c0d");
        }
    }
}
