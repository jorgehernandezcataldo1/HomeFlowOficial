using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeFlowOficial.Migrations
{
    /// <inheritdoc />
    public partial class Nuevo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personas_Empresas_EmpresaId",
                table: "Personas");

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Inmuebles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Empresas",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Empresas",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Inmuebles_EmpresaId",
                table: "Inmuebles",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_Rut",
                table: "Empresas",
                column: "Rut",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmpresaId",
                table: "AspNetUsers",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Empresas_EmpresaId",
                table: "AspNetUsers",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inmuebles_Empresas_EmpresaId",
                table: "Inmuebles",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Personas_Empresas_EmpresaId",
                table: "Personas",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Empresas_EmpresaId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Inmuebles_Empresas_EmpresaId",
                table: "Inmuebles");

            migrationBuilder.DropForeignKey(
                name: "FK_Personas_Empresas_EmpresaId",
                table: "Personas");

            migrationBuilder.DropIndex(
                name: "IX_Inmuebles_EmpresaId",
                table: "Inmuebles");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_Rut",
                table: "Empresas");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmpresaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Inmuebles");

            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_Personas_Empresas_EmpresaId",
                table: "Personas",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
