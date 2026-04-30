using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataAccessInterface
{
    public class SqlDataAccess : IDataAccess
    {
        public async Task<DataTable> ExecuteQueryAsync(string connectionString, string sql, IEnumerable<SqlParameter>? parameters = null)
        {
            var dt = new DataTable();
            await using var conn = new SqlConnection(connectionString);
            await using var cmd = new SqlCommand(sql, conn)
            {
                CommandType = CommandType.Text
            };

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters.ToArray());
            }

            await conn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();
            dt.Load(reader);
            return dt;
        }

        public async Task<object?> ExecuteScalarAsync(string connectionString, string sql, IEnumerable<SqlParameter>? parameters = null)
        {
            await using var conn = new SqlConnection(connectionString);
            await using var cmd = new SqlCommand(sql, conn)
            {
                CommandType = CommandType.Text
            };
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters.ToArray());
            }
            await conn.OpenAsync();
            return await cmd.ExecuteScalarAsync();
        }

        public async Task<int> ExecuteNonQueryAsync(string connectionString, string sql, IEnumerable<SqlParameter>? parameters = null)
        {
            await using var conn = new SqlConnection(connectionString);
            await using var cmd = new SqlCommand(sql, conn)
            {
                CommandType = CommandType.Text
            };
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters.ToArray());
            }
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<DataTable> ExecuteStoredProcedureAsync(string connectionString, string procedureName, IEnumerable<SqlParameter>? parameters = null)
        {
            var dt = new DataTable();
            await using var conn = new SqlConnection(connectionString);
            await using var cmd = new SqlCommand(procedureName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters.ToArray());
            }
            await conn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();
            dt.Load(reader);
            return dt;
        }

    }
}
