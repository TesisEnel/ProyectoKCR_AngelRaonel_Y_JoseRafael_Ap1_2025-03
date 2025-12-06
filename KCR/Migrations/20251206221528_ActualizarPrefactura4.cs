using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KCR.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarPrefactura4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "servicios",
                columns: new[] { "IdServicio", "Nombre", "Precio", "Tipo" },
                values: new object[,]
                {
                    { 18, "SERVICIO EXPRESS", 0.0, null },
                    { 19, "DISEÑO Y EDICIÓN", 0.0, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "servicios",
                keyColumn: "IdServicio",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "servicios",
                keyColumn: "IdServicio",
                keyValue: 19);
        }
    }
}
