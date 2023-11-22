using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AjusteTipoDato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 11, 22, 10, 54, 5, 354, DateTimeKind.Local).AddTicks(2815), new DateTime(2023, 11, 22, 10, 54, 5, 354, DateTimeKind.Local).AddTicks(2802) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 11, 22, 10, 54, 5, 354, DateTimeKind.Local).AddTicks(2819), new DateTime(2023, 11, 22, 10, 54, 5, 354, DateTimeKind.Local).AddTicks(2818) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 11, 22, 10, 2, 3, 13, DateTimeKind.Local).AddTicks(5979), new DateTime(2023, 11, 22, 10, 2, 3, 13, DateTimeKind.Local).AddTicks(5965) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 11, 22, 10, 2, 3, 13, DateTimeKind.Local).AddTicks(5983), new DateTime(2023, 11, 22, 10, 2, 3, 13, DateTimeKind.Local).AddTicks(5982) });
        }
    }
}
