using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events; // Para LogEventLevel
// Importar solo las clases necesarias directamente para evitar ambigüedades
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.PostgreSQL;
using Serilog.Sinks.MySQL;
using System;
using System.Collections.Generic; // Para Dictionary<string, string> en MySQL

namespace ANPRVisionAPI.Extensions
{
    public static class SerilogConfigurationExtension
    {
        /// <summary>
        /// Configura Serilog para el logging en base de datos de forma dinámica.
        /// </summary>
        /// <param name="loggerConfiguration">La configuración actual de Serilog.</param>
        /// <param name="configuration">La configuración de la aplicación (appsettings.json).</param>
        /// <returns>La configuración de Serilog actualizada.</returns>
        public static LoggerConfiguration AddDatabaseLogging(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            // Obtener el tipo de base de datos desde la configuración
            var databaseType = configuration.GetValue<string>("DatabaseSettings:DatabaseType");

            // Inicializamos con string.Empty para evitar advertencias de nulabilidad si es el caso.
            string connectionString = string.Empty;
            string tableName = "Logs"; // Nombre de tabla común para los logs

            if (databaseType?.Equals("postgresql", StringComparison.OrdinalIgnoreCase) == true)
            {
                connectionString = configuration.GetConnectionString("LocalPostgres") ?? string.Empty; // Usar ?? string.Empty para seguridad
                tableName = "Logs_PostgreSQL"; // Nombre de tabla específico para este DB
                string schemaName = "public";

                loggerConfiguration.WriteTo.PostgreSQL(
                    connectionString: connectionString,
                    tableName: tableName,
                    schemaName: schemaName,
                    needAutoCreateSqlTable: false, // Asume que la tabla ya está creada manualmente
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    period: TimeSpan.FromSeconds(5),
                    batchSizeLimit: 100,
                    // Usar Serilog.Sinks.PostgreSQL.ColumnOptions si hay ambigüedad,
                    // pero con el using correcto, debería ser suficiente.
                    columnOptions: new Serilog.Sinks.PostgreSQL.ColumnOptions
                    {
                        AdditionalColumns = new List<Serilog.Sinks.PostgreSQL.ColumnOptions.Column>
                        {
                            new Serilog.Sinks.PostgreSQL.ColumnOptions.Column { ColumnName = "MessageTemplate", DataType = Serilog.Sinks.PostgreSQL.ColumnOptions.Enum.DataType.Text },
                            new Serilog.Sinks.PostgreSQL.ColumnOptions.Column { ColumnName = "Exception", DataType = Serilog.Sinks.PostgreSQL.ColumnOptions.Enum.DataType.Text },
                            new Serilog.Sinks.PostgreSQL.ColumnOptions.Column { ColumnName = "Properties", DataType = Serilog.Sinks.PostgreSQL.ColumnOptions.Enum.DataType.Jsonb },
                            new Serilog.Sinks.PostgreSQL.ColumnOptions.Column { ColumnName = "LogEvent", DataType = Serilog.Sinks.PostgreSQL.ColumnOptions.Enum.DataType.Jsonb }
                        }
                    }
                );
                Console.WriteLine($"Serilog configured for PostgreSQL logging to: {connectionString}");
            }
            else if (databaseType?.Equals("mysql", StringComparison.OrdinalIgnoreCase) == true)
            {
                connectionString = configuration.GetConnectionString("MySqlConnection") ?? string.Empty;
                tableName = "Logs_MySQL"; // Nombre de tabla específico para este DB

                loggerConfiguration.WriteTo.MySQL(
                    connectionString: connectionString,
                    tableName: tableName,
                    // ELIMINADO: autoCreateSqlTable no existe en Serilog.Sinks.MySQL 5.x
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    columnOptions: new Dictionary<string, string> // Usar diccionario para MySQL columnOptions
                    {
                        { "Id", "INT AUTO_INCREMENT PRIMARY KEY" },
                        { "Message", "TEXT" },
                        { "MessageTemplate", "TEXT" },
                        { "Level", "VARCHAR(128)" },
                        { "TimeStamp", "DATETIME" },
                        { "Exception", "TEXT" },
                        { "Properties", "JSON" },
                        { "LogEvent", "JSON" }
                    }
                );
                Console.WriteLine($"Serilog configured for MySQL logging to: {connectionString}");
            }
            else if (databaseType?.Equals("sqlserver", StringComparison.OrdinalIgnoreCase) == true)
            {
                connectionString = configuration.GetConnectionString("SqlServerConnection") ?? string.Empty;
                tableName = "Logs_SqlServer"; // Nombre de tabla específico para este DB
                string schemaName = "dbo";

                var columnOptions = new Serilog.Sinks.MSSqlServer.ColumnOptions();
                // Asegúrate que las columnas estándar para propiedades y eventos se almacenen correctamente
                // Las propiedades por defecto ya incluyen Message, Level, TimeStamp, Exception.
                // Aseguramos que Properties y LogEvent se incluyan si fueron removidos o no son los defaults.
                columnOptions.Store.Add(StandardColumn.LogEvent);
                columnOptions.Store.Add(StandardColumn.Properties);

                loggerConfiguration.WriteTo.MSSqlServer(
                    connectionString: connectionString,
                    tableName: tableName,
                    schemaName: schemaName,
                    autoCreateSqlTable: false, // Asume que la tabla ya está creada manualmente
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    columnOptions: columnOptions
                );
                Console.WriteLine($"Serilog configured for SQL Server logging to: {connectionString}");
            }
            else
            {
                // Fallback a consola si no hay DB configurada o el tipo no es válido
                Console.WriteLine("ADVERTENCIA: No se especificó un tipo de base de datos válido para logging o la cadena de conexión no se encontró. Se usará el log de consola por defecto.");
                // No se agrega un sink de DB en este bloque, solo el de consola que se añade al final.
            }

            // Siempre enviar logs a la consola, además de la DB seleccionada (útil en desarrollo)
            loggerConfiguration.WriteTo.Console();

            return loggerConfiguration;
        }
    }
}