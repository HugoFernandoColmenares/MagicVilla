using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalles de la villa", new DateTime(2023, 11, 22, 10, 2, 3, 13, DateTimeKind.Local).AddTicks(5979), new DateTime(2023, 11, 22, 10, 2, 3, 13, DateTimeKind.Local).AddTicks(5965), "", 50, "Nombre Real", 5, 200.0 },
                    { 2, "", "Detalles de la villa", new DateTime(2023, 11, 22, 10, 2, 3, 13, DateTimeKind.Local).AddTicks(5983), new DateTime(2023, 11, 22, 10, 2, 3, 13, DateTimeKind.Local).AddTicks(5982), "", 40, "Premium Vista a la Piscina", 4, 150.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
