using DataAccessInterface;
using LoggingInterface.Interface;
using LoggingInterface.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

public class DBConnection : IDBConnection
{
    private readonly string _connStr;
    private readonly IDataAccess _dataAccess;

    public DBConnection(IConfiguration configuration, IDataAccess dataAccess)
    {
        _connStr = configuration.GetConnectionString("DefaultConnection");
        _dataAccess = dataAccess;
    }

    public async Task SaveLogAsync(LogModel log)
    {
        try
        {
            // 🔹 Trim large data (important)
            var requestBody = log.RequestBody?.Length > 5000
                ? log.RequestBody.Substring(0, 5000)
                : log.RequestBody;

            var responseBody = log.ResponseBody?.Length > 5000
                ? log.ResponseBody.Substring(0, 5000)
                : log.ResponseBody;

            var parameters = new List<SqlParameter>
        {
            new SqlParameter("@Path", log.Path ?? (object)DBNull.Value),
            new SqlParameter("@Method", log.Method ?? (object)DBNull.Value),
            new SqlParameter("@RequestBody", requestBody ?? (object)DBNull.Value),
            new SqlParameter("@ResponseBody", responseBody ?? (object)DBNull.Value),
            new SqlParameter("@StatusCode", log.StatusCode),

            // ✅ Optional but recommended
            new SqlParameter("@IPAddress", log.IPAddress ?? (object)DBNull.Value),
            new SqlParameter("@UserId", log.UserId ?? (object)DBNull.Value),
            new SqlParameter("@ExecutionTimeMs", log.ExecutionTimeMs),

            new SqlParameter("@CreatedAt", log.CreatedAt == default
                ? DateTime.Now
                : log.CreatedAt)
        };

            await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "sp_SaveApiLog",
                parameters
            );
        }
        catch (Exception ex)
        {
            // Logging should never crash API
            Console.WriteLine("Log Save Error: " + ex.Message);
        }
    }
}