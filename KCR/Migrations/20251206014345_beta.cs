using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KCR.Migrations
{
    /// <inheritdoc />
    public partial class beta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_preFacturas_empleados_IdEmpleado",
                table: "preFacturas");

            migrationBuilder.DropIndex(
                name: "IX_preFacturas_IdEmpleado",
                table: "preFacturas");

            migrationBuilder.DropColumn(
                name: "IdEmpleado",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "IdEmpleado",
                table: "preFacturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_preFacturas_IdEmpleado",
                table: "preFacturas",
                column: "IdEmpleado");

            migrationBuilder.AddForeignKey(
                name: "FK_preFacturas_empleados_IdEmpleado",
                table: "preFacturas",
                column: "IdEmpleado",
                principalTable: "empleados",
                principalColumn: "IdEmpleado",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
