using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Back.Migrations
{
    /// <inheritdoc />
    public partial class add_user_neilos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Candidates",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "CreatedAt", "Email", "FirstName", "LastName", "Password", "Phone", "Role" },
                values: new object[] { 4, "Nea smirni", new DateTime(2023, 12, 28, 18, 58, 37, 651, DateTimeKind.Local).AddTicks(770), "", "Neilos", "Kotsiopoulos", "AQAAAAIAAYagAAAAEPyPqDdsy1Dm0/9ha5foebLh3wvlwuycOtrqQVXdq66uW14eYgIKOaypZHfkANnKCQ==", "123", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "BirthDate",
                table: "Candidates",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
