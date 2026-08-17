using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeFlowOficial.Migrations
{
    /// <inheritdoc />
    public partial class Restruccturacioonperra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rut",
                table: "AspNetUsers",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rut",
                table: "AspNetUsers");
        }
    }
}
