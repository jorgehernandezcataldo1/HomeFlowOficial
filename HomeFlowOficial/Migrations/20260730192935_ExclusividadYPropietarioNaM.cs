using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeFlowOficial.Migrations
{
    /// <inheritdoc />
    public partial class ExclusividadYPropietarioNaM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Propietarios_Personas_PersonaId",
                table: "Propietarios");

            migrationBuilder.DropIndex(
                name: "IX_Propietarios_PersonaId",
                table: "Propietarios");

            migrationBuilder.AddColumn<bool>(
                name: "TieneExclusividad",
                table: "Inmuebles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Propietarios_PersonaId_CorredorId",
                table: "Propietarios",
                columns: new[] { "PersonaId", "CorredorId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Propietarios_Personas_PersonaId",
                table: "Propietarios",
                column: "PersonaId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Propietarios_Personas_PersonaId",
                table: "Propietarios");

            migrationBuilder.DropIndex(
                name: "IX_Propietarios_PersonaId_CorredorId",
                table: "Propietarios");

            migrationBuilder.DropColumn(
                name: "TieneExclusividad",
                table: "Inmuebles");

            migrationBuilder.CreateIndex(
                name: "IX_Propietarios_PersonaId",
                table: "Propietarios",
                column: "PersonaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Propietarios_Personas_PersonaId",
                table: "Propietarios",
                column: "PersonaId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
