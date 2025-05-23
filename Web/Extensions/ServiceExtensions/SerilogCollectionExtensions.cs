using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.MySQL;
using System;
using System.Collections.Generic;
// Asegúrate de este using para los ColumnWriters de Serilog.Sinks.Postgresql.Alternative
using Serilog.Sinks.PostgreSQL.ColumnWriters;
// También necesitarás esto para NpgsqlDbType si no lo tienes ya
using NpgsqlTypes;

namespace ANPRVisionAPI.Extensions
{
    public static class SerilogConfigurationExtension
    {
        public static LoggerConfiguration AddDatabaseLogging(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            var databaseType = configuration.GetValue<string>("DatabaseSettings:DatabaseType");
            string connectionString = string.Empty;
            string tableName = "Logs";

            if (databaseType?.Equals("postgresql", StringComparison.OrdinalIgnoreCase) == true)
            {
                connectionString = configuration.GetConnectionString("LocalPostgres") ?? string.Empty;
                tableName = "Logs_PostgreSQL";
                string schemaName = "public";

                // Define las columnas como un Dictionary<string, ColumnWriterBase>
                var columns = new Dictionary<string, ColumnWriterBase>
                {
                    { "Message", new MessageTemplateColumnWriter() }, // 'Message' suele ser el nombre por defecto para el mensaje formateado
                    { "Level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) }, // Para guardar el nivel (Information, Warning, etc.)
                    { "TimeStamp", new TimestampColumnWriter() }, // Para la fecha y hora
                    { "Exception", new ExceptionColumnWriter() },
                    { "Properties", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
                    { "LogEvent", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) }
                };

                loggerConfiguration.WriteTo.PostgreSQL(
                   connectionString: connectionString,
                   tableName: tableName,
                   needAutoCreateTable: false, // Asegúrate de que tu tabla ya existe o cámbialo a true
                   schemaName: schemaName,
                   restrictedToMinimumLevel: LogEventLevel.Information,
                   period: TimeSpan.FromSeconds(5),
                   batchSizeLimit: 100,
                   // El parámetro correcto es 'columns' en Serilog.Sinks.Postgresql.Alternative
                   columns: columns
                );

                Console.WriteLine($"Serilog configured for PostgreSQL logging to: {connectionString}");
            }
            else if (databaseType?.Equals("mysql", StringComparison.OrdinalIgnoreCase) == true)
            {
                connectionString = configuration.GetConnectionString("MySqlConnection") ?? string.Empty;
                tableName = "Logs_MySQL";

                loggerConfiguration.WriteTo.MySQL(
                    connectionString: connectionString,
                    tableName: tableName,
                    restrictedToMinimumLevel: LogEventLevel.Information
                );
                Console.WriteLine($"Serilog configured for MySQL logging to: {connectionString}");
            }
            else if (databaseType?.Equals("sqlserver", StringComparison.OrdinalIgnoreCase) == true)
            {
                connectionString = configuration.GetConnectionString("SqlServerConnection") ?? string.Empty;
                tableName = "Logs_SqlServer";
                string schemaName = "dbo";

                var columnOptions = new Serilog.Sinks.MSSqlServer.ColumnOptions();
                columnOptions.Store.Add(StandardColumn.LogEvent);
                columnOptions.Store.Add(StandardColumn.Properties);

                loggerConfiguration.WriteTo.MSSqlServer(
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = tableName,
                        SchemaName = schemaName,
                        AutoCreateSqlTable = false
                    },
                    connectionString: connectionString,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    columnOptions: columnOptions
                );
                Console.WriteLine($"Serilog configured for SQL Server logging to: {connectionString}");
            }
            else
            {
                Console.WriteLine("ADVERTENCIA: No se especificó un tipo de base de datos válido para logging o la cadena de conexión no se encontró. Se usará el log de consola por defecto.");
            }

            loggerConfiguration.WriteTo.Console();

            return loggerConfiguration;
        }
    }
}