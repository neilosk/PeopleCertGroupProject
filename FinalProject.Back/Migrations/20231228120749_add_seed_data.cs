using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinalProject.Back.Migrations
{
    /// <inheritdoc />
    public partial class add_seed_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Certificates",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Microsoft Certified: Azure Fundamentals" },
                    { 2, "Microsoft Certified: Azure Administrator Associate" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Certificates",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Certificates",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
