using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeFlowOficial.Migrations
{
    /// <inheritdoc />
    public partial class Entidadesssssss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioIngresoId",
                table: "Propietarios",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Propietarios_UsuarioIngresoId",
                table: "Propietarios",
                column: "UsuarioIngresoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Propietarios_AspNetUsers_UsuarioIngresoId",
                table: "Propietarios",
                column: "UsuarioIngresoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Propietarios_AspNetUsers_UsuarioIngresoId",
                table: "Propietarios");

            migrationBuilder.DropIndex(
                name: "IX_Propietarios_UsuarioIngresoId",
                table: "Propietarios");

            migrationBuilder.DropColumn(
                name: "UsuarioIngresoId",
                table: "Propietarios");
        }
    }
}
