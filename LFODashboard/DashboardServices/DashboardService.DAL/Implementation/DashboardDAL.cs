using DataAccessInterface;
using DashboardService.DAL.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DashboardService.DAL.Implementation
{
    public class DashboardDAL : IDashboardDAL
    {
        private readonly string _connStr;
        private readonly IDataAccess _dataAccess;

        public DashboardDAL(IConfiguration configuration, IDataAccess dataAccess)
        {
            _connStr = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Connection string is null");
            _dataAccess = dataAccess;
        }

        public async Task<DataTable> GetDashboardDataAsync()
        {
            var parameters = new List<SqlParameter>();
            var result = await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "usp_GetDashboardData", parameters);
            return result;
        }
    }
}
