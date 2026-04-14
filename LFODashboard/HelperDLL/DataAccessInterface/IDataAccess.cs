using Microsoft.Data.SqlClient;
using System.Data;

namespace DataAccessInterface
{
    public interface IDataAccess
    {
        // Execute a query and return a DataTable
        Task<DataTable> ExecuteQueryAsync(string connectionString, string sql, IEnumerable<SqlParameter>? parameters = null);

        // Execute a scalar query and return the first column of the first row
        Task<object?> ExecuteScalarAsync(string connectionString, string sql, IEnumerable<SqlParameter>? parameters = null);

        // Execute a non-query (INSERT/UPDATE/DELETE) and return affected rows
        Task<int> ExecuteNonQueryAsync(string connectionString, string sql, IEnumerable<SqlParameter>? parameters = null);

        // Execute a stored procedure and return a DataTable
        Task<DataTable> ExecuteStoredProcedureAsync(string connectionString, string procedureName, IEnumerable<SqlParameter>? parameters = null);
    }

    
}
