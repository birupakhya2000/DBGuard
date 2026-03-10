using DBGuard.Core.Interfaces;
using DBGuard.Core.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.SqlServer.Providers
{
    public class SqlServerSchemaProvider : ISchemaProvider
    {
        public async Task<DatabaseSchema> LoadSchemaAsync(string connectionString)
        {
            var schema = new DatabaseSchema();

            var query = @"
            SELECT 
                TABLE_NAME,
                COLUMN_NAME,
                DATA_TYPE,
                CHARACTER_MAXIMUM_LENGTH,
                IS_NULLABLE,
                NUMERIC_PRECISION,
                NUMERIC_SCALE
            FROM INFORMATION_SCHEMA.COLUMNS
        ";

            using var connection = new SqlConnection(connectionString);

            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var tableName = reader["TABLE_NAME"].ToString();
                var columnName = reader["COLUMN_NAME"].ToString();

                var dataType = reader["DATA_TYPE"].ToString();

                var maxLength = reader["CHARACTER_MAXIMUM_LENGTH"] as int?;
                var isNullable = reader["IS_NULLABLE"].ToString() == "YES";

                var precision = reader["NUMERIC_PRECISION"] as byte?;
                var scale = reader["NUMERIC_SCALE"] as int?;

                if (!schema.Tables.TryGetValue(tableName, out var table))
                {
                    table = new TableSchema
                    {
                        TableName = tableName
                    };

                    schema.Tables.Add(tableName, table);
                }

                var column = new ColumnSchema
                {
                    TableName = tableName,
                    ColumnName = columnName,
                    DataType = dataType,
                    MaxLength = maxLength,
                    IsNullable = isNullable,
                    Precision = precision,
                    Scale = scale
                };

                table.Columns[columnName] = column;
            }

            return schema;
        }
    }
}
