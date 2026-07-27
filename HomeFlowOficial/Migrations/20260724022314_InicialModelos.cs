using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeFlowOficial.Migrations
{
    /// <inheritdoc />
    public partial class InicialModelos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Propietarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioIngresoId",
                table: "Propietarios",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PersonaRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonaId = table.Column<int>(type: "int", nullable: false),
                    TipoRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonaRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonaRoles_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Propietarios_UsuarioIngresoId",
                table: "Propietarios",
                column: "UsuarioIngresoId");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_Correo",
                table: "Personas",
                column: "Correo");

            migrationBuilder.CreateIndex(
                name: "IX_PersonaRoles_PersonaId",
                table: "PersonaRoles",
                column: "PersonaId");

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

            migrationBuilder.DropTable(
                name: "PersonaRoles");

            migrationBuilder.DropIndex(
                name: "IX_Propietarios_UsuarioIngresoId",
                table: "Propietarios");

            migrationBuilder.DropIndex(
                name: "IX_Personas_Correo",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Propietarios");

            migrationBuilder.DropColumn(
                name: "UsuarioIngresoId",
                table: "Propietarios");
        }
    }
}
