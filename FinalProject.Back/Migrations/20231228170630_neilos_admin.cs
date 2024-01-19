using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Back.Migrations
{
    /// <inheritdoc />
    public partial class neilos_admin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "CreatedAt", "Email", "FirstName", "LastName", "Password", "Phone", "Role" },
                values: new object[] { 1, "Nea Smirni", new DateTime(2023, 12, 28, 19, 6, 30, 591, DateTimeKind.Local).AddTicks(2391), "neilos@neko.com", "Neilos", "Kotsiopoulos", "AQAAAAIAAYagAAAAEPyPqDdsy1Dm0/9ha5foebLh3wvlwuycOtrqQVXdq66uW14eYgIKOaypZHfkANnKCQ==", "123", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "CreatedAt", "Email", "FirstName", "LastName", "Password", "Phone", "Role" },
                values: new object[] { 4, "Nea smirni", new DateTime(2023, 12, 28, 18, 59, 27, 135, DateTimeKind.Local).AddTicks(4263), "neilos@neko.com", "Neilos", "Kotsiopoulos", "AQAAAAIAAYagAAAAEPyPqDdsy1Dm0/9ha5foebLh3wvlwuycOtrqQVXdq66uW14eYgIKOaypZHfkANnKCQ==", "123", "admin" });
        }
    }
}
