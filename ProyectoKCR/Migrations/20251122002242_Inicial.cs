using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoKCR.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clientes",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Cedula = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Telefono = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "empleados",
                columns: table => new
                {
                    IdEmpleado = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Usuario = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Clave = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Cedula = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Cargo = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empleados", x => x.IdEmpleado);
                });

            migrationBuilder.CreateTable(
                name: "materiales",
                columns: table => new
                {
                    IdMaterial = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Existencia = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materiales", x => x.IdMaterial);
                });

            migrationBuilder.CreateTable(
                name: "turnos",
                columns: table => new
                {
                    IdTurno = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumTurno = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Servicio = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IdCliente = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_turnos", x => x.IdTurno);
                    table.ForeignKey(
                        name: "FK_turnos_clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "preFacturas",
                columns: table => new
                {
                    IdPreFactura = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NombreCliente = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IdCliente = table.Column<int>(type: "INTEGER", nullable: false),
                    IdEmpleado = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_preFacturas", x => x.IdPreFactura);
                    table.ForeignKey(
                        name: "FK_preFacturas_clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_preFacturas_empleados_IdEmpleado",
                        column: x => x.IdEmpleado,
                        principalTable: "empleados",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detallePreFactura",
                columns: table => new
                {
                    IdDetalle = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Servicio = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecioUnitarioHistorico = table.Column<decimal>(type: "TEXT", nullable: false),
                    IdMaterial = table.Column<int>(type: "INTEGER", nullable: false),
                    IdPreFactura = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detallePreFactura", x => x.IdDetalle);
                    table.ForeignKey(
                        name: "FK_detallePreFactura_materiales_IdMaterial",
                        column: x => x.IdMaterial,
                        principalTable: "materiales",
                        principalColumn: "IdMaterial",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_detallePreFactura_preFacturas_IdPreFactura",
                        column: x => x.IdPreFactura,
                        principalTable: "preFacturas",
                        principalColumn: "IdPreFactura",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "materiales",
                columns: new[] { "IdMaterial", "Existencia", "Nombre", "PrecioUnitario" },
                values: new object[] { 1, 100, "Papel", 5m });

            migrationBuilder.CreateIndex(
                name: "IX_detallePreFactura_IdMaterial",
                table: "detallePreFactura",
                column: "IdMaterial");

            migrationBuilder.CreateIndex(
                name: "IX_detallePreFactura_IdPreFactura",
                table: "detallePreFactura",
                column: "IdPreFactura");

            migrationBuilder.CreateIndex(
                name: "IX_preFacturas_IdCliente",
                table: "preFacturas",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_preFacturas_IdEmpleado",
                table: "preFacturas",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_turnos_IdCliente",
                table: "turnos",
                column: "IdCliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "detallePreFactura");

            migrationBuilder.DropTable(
                name: "turnos");

            migrationBuilder.DropTable(
                name: "materiales");

            migrationBuilder.DropTable(
                name: "preFacturas");

            migrationBuilder.DropTable(
                name: "clientes");

            migrationBuilder.DropTable(
                name: "empleados");
        }
    }
}
