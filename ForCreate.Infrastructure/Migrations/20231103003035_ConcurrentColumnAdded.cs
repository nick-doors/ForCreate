using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForCreate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConcurrentColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VersionId",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VersionId",
                table: "Company",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VersionId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "VersionId",
                table: "Company");
        }
    }
}
