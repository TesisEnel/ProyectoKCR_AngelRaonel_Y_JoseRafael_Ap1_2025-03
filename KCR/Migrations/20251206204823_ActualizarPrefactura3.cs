using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KCR.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarPrefactura3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_preFacturas_empleados_EmpleadosIdEmpleado",
                table: "preFacturas");

            migrationBuilder.DropIndex(
                name: "IX_preFacturas_EmpleadosIdEmpleado",
                table: "preFacturas");

            migrationBuilder.DropColumn(
                name: "EmpleadosIdEmpleado",
                table: "preFacturas");

            migrationBuilder.AddColumn<int>(
                name: "EmpleadoIdEmpleado",
                table: "preFacturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpleado",
                table: "preFacturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdTurno",
                table: "preFacturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TurnoIdTurno",
                table: "preFacturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_preFacturas_EmpleadoIdEmpleado",
                table: "preFacturas",
                column: "EmpleadoIdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_preFacturas_TurnoIdTurno",
                table: "preFacturas",
                column: "TurnoIdTurno");

            migrationBuilder.AddForeignKey(
                name: "FK_preFacturas_empleados_EmpleadoIdEmpleado",
                table: "preFacturas",
                column: "EmpleadoIdEmpleado",
                principalTable: "empleados",
                principalColumn: "IdEmpleado",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_preFacturas_turnos_TurnoIdTurno",
                table: "preFacturas",
                column: "TurnoIdTurno",
                principalTable: "turnos",
                principalColumn: "IdTurno",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_preFacturas_empleados_EmpleadoIdEmpleado",
                table: "preFacturas");

            migrationBuilder.DropForeignKey(
                name: "FK_preFacturas_turnos_TurnoIdTurno",
                table: "preFacturas");

            migrationBuilder.DropIndex(
                name: "IX_preFacturas_EmpleadoIdEmpleado",
                table: "preFacturas");

            migrationBuilder.DropIndex(
                name: "IX_preFacturas_TurnoIdTurno",
                table: "preFacturas");

            migrationBuilder.DropColumn(
                name: "EmpleadoIdEmpleado",
                table: "preFacturas");

            migrationBuilder.DropColumn(
                name: "IdEmpleado",
                table: "preFacturas");

            migrationBuilder.DropColumn(
                name: "IdTurno",
                table: "preFacturas");

            migrationBuilder.DropColumn(
                name: "TurnoIdTurno",
                table: "preFacturas");

            migrationBuilder.AddColumn<int>(
                name: "EmpleadosIdEmpleado",
                table: "preFacturas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_preFacturas_EmpleadosIdEmpleado",
                table: "preFacturas",
                column: "EmpleadosIdEmpleado");

            migrationBuilder.AddForeignKey(
                name: "FK_preFacturas_empleados_EmpleadosIdEmpleado",
                table: "preFacturas",
                column: "EmpleadosIdEmpleado",
                principalTable: "empleados",
                principalColumn: "IdEmpleado");
        }
    }
}
