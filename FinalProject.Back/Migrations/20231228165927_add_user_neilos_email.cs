using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Back.Migrations
{
    /// <inheritdoc />
    public partial class add_user_neilos_email : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Email" },
                values: new object[] { new DateTime(2023, 12, 28, 18, 59, 27, 135, DateTimeKind.Local).AddTicks(4263), "neilos@neko.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Email" },
                values: new object[] { new DateTime(2023, 12, 28, 18, 58, 37, 651, DateTimeKind.Local).AddTicks(770), "" });
        }
    }
}
