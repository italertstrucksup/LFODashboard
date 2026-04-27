using DataAccessInterface;
using ManageAccessService.DAL.Interface;
using ManageAccessService.Model.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ManageAccessService.DAL.Implementation
{
    public class PermissionDAL : IPermissionDAL
    {
        private readonly string _connStr;
        private readonly IDataAccess _dataAccess;
        public PermissionDAL(IConfiguration configuration, IDataAccess dataAccess)
        {
            _connStr = configuration.GetConnectionString("DefaultConnection");
            _dataAccess = dataAccess;
        }

        public async Task<DataTable> GetModules(PermissionRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", (object?)request.UserId ?? DBNull.Value),
                new SqlParameter("@AccessType", (object?)request.AccessType ?? DBNull.Value),
                new SqlParameter("@Action", "modules")
            };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "usp_GetPermission",
                parameters
            );

            return result;
        }

        public async Task<DataTable> GetFeatures(PermissionRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", (object?)request.UserId ?? DBNull.Value),
                new SqlParameter("@AccessType", (object?)request.AccessType ?? DBNull.Value),
                new SqlParameter("@ModuleId", (object?)request.ModuleId ?? DBNull.Value),
                new SqlParameter("@Action", "features")
            };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "usp_GetPermission",
                parameters
            );

            return result;
        }

    }
}
