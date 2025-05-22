using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Entity.Context.Migrations // ¡VERIFICA ESTE NAMESPACE! Debería coincidir con la ubicación real del archivo de migración.
{
    /// <inheritdoc />
    public partial class SeedInitialRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ***** LÓGICA PARA INSERTAR DATOS INICIALES EN LA TABLA 'ROL' *****
            // Asegúrate de que el nombre de la tabla "rol" y los nombres de las columnas
            // ("id", "name", "description", "active") coincidan exactamente
            // con los nombres reales en tu base de datos (sensibilidad a mayúsculas/minúsculas).
            migrationBuilder.InsertData(
                table: "rol", // <-- Nombre de la tabla en tu base de datos
                columns: new[] { "id", "name", "description", "active" }, // <-- Nombres de las columnas en tu base de datos
                values: new object[,]
                {
                    { 1, "Administrador", "Rol con acceso completo al sistema", true },
                    { 2, "Usuario", "Rol con permisos estándar", true },
                    { 3, "Invitado", "Rol con acceso limitado", true }
                });

            // Si tienes más tablas que necesitan datos iniciales (ej. una configuración inicial, tipos de cámaras),
            // puedes añadir más llamadas a migrationBuilder.InsertData aquí.
            // Por ejemplo, para la tabla 'Camara' (si necesitas algunas cámaras predefinidas):
            /*
            migrationBuilder.InsertData(
                table: "Camara", // Asegúrate del nombre exacto de la tabla 'Camara' en tu DB
                columns: new[] { "id", "nombre", "ubicacion" }, // Asegúrate de los nombres exactos de las columnas
                values: new object[,]
                {
                    { 1, "Camara Entrada Principal", "Edificio A - Entrada" },
                    { 2, "Camara Parqueadero", "Estacionamiento Oeste" }
                });
            */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // ***** LÓGICA PARA ELIMINAR LOS DATOS INICIALES (si se revierte la migración) *****
            // Esto es importante para poder deshacer la migración limpiamente.
            migrationBuilder.DeleteData(
                table: "rol",
                keyColumn: "id",
                keyValues: new object[] { 1 });

            migrationBuilder.DeleteData(
                table: "rol",
                keyColumn: "id",
                keyValues: new object[] { 2 });

            migrationBuilder.DeleteData(
                table: "rol",
                keyColumn: "id",
                keyValues: new object[] { 3 });

            // Eliminar datos para otras tablas si los insertaste en Up(). Por ejemplo, para 'Camara':
            /*
            migrationBuilder.DeleteData(
                table: "Camara",
                keyColumn: "id",
                keyValues: new object[] { 1 });

            migrationBuilder.DeleteData(
                table: "Camara",
                keyColumn: "id",
                keyValues: new object[] { 2 });
            */
        }
    }
}