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
    public class AccessDAL : IAccessDAL
    {
        private readonly string _connStr;
        private readonly IDataAccess _dataAccess;
        public AccessDAL(IConfiguration configuration, IDataAccess dataAccess)
        {
            _connStr = configuration.GetConnectionString("DefaultConnection");
            _dataAccess = dataAccess;
        }

        public async Task<DataTable> GetRoleAsync()
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "roles")
            };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "usp_GetPermission",
                parameters
            );

            return result;
        }

        public async Task<UserApiResponse> AddUserAsync(UserApiRequest request)
        {
            var parameters = new List<SqlParameter>
            {
               //new SqlParameter("@tenant_id", request.TenantId),
                //new SqlParameter("@public_id", (object?)request.PublicId ?? DBNull.Value),
                new SqlParameter("@user_name", (object?)request.UserName ?? DBNull.Value),
                new SqlParameter("@mobile_number", request.MobileNo),
                //new SqlParameter("@password_hash", (object?)request.PasswordHash ?? DBNull.Value),
                //new SqlParameter("@is_active", request.IsActive),
                //new SqlParameter("@is_blocked", request.IsBlocked),
                //new SqlParameter("@device_info", (object?)request.DeviceInfo ?? DBNull.Value),
                new SqlParameter("@role_id", (object?)request.AccessType ?? DBNull.Value),

                new SqlParameter("@admin_user_id", (object?)request.AdminUserId ?? DBNull.Value),
                new SqlParameter("@created_at", DBNull.Value) // let DB handle it
            };

            var dt = await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "usp_InsertAuthUser",
                parameters
            );

            // ✅ Map result
            var response = new UserApiResponse();

            if (dt.Rows.Count > 0)
            {
                response.UserId = dt.Rows[0]["id"] != DBNull.Value
                    ? dt.Rows[0]["id"].ToString()
                    : null;

                response.Message = dt.Rows[0]["message"]?.ToString();
            }

            return response;
        }
        public async Task<UserApiResponse> EditUserAsync(UserApiRequest request)
        {
            var parameters = new List<SqlParameter>
            {
               //new SqlParameter("@tenant_id", request.TenantId),
                new SqlParameter("@id", (object?)request.UserId ?? DBNull.Value),
                new SqlParameter("@user_name", (object?)request.UserName ?? DBNull.Value),
                new SqlParameter("@mobile_number", request.MobileNo),
                //new SqlParameter("@password_hash", (object?)request.PasswordHash ?? DBNull.Value),
                //new SqlParameter("@is_active", request.IsActive),
                //new SqlParameter("@is_blocked", request.IsBlocked),
                //new SqlParameter("@device_info", (object?)request.DeviceInfo ?? DBNull.Value),
                new SqlParameter("@role_id", (object?)request.AccessType ?? DBNull.Value),
                new SqlParameter("@admin_user_id", (object?)request.AdminUserId ?? DBNull.Value)
            };

            var dt = await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "usp_UpdateAuthUser",
                parameters
            );

            // ✅ Map result
            var response = new UserApiResponse();

            if (dt.Rows.Count > 0)
            {
                response.UserId = dt.Rows[0]["id"] != DBNull.Value ? dt.Rows[0]["id"].ToString() : null;
                response.Message = dt.Rows[0]["message"]?.ToString();
            }

            return response;
        }
        
        public async Task<UserApiResponse> DeleteUserAsync(UserApiRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@id", (object?)request.UserId ?? DBNull.Value),
                new SqlParameter("@is_active", 0),
                new SqlParameter("@is_blocked", 1)
            };

            var dt = await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "usp_UpdateAuthUser",
                parameters
            );

            // ✅ Map result
            var response = new UserApiResponse();

            if (dt.Rows.Count > 0)
            {
                response.UserId = dt.Rows[0]["id"] != DBNull.Value ? dt.Rows[0]["id"].ToString() : null;
                response.Message = dt.Rows[0]["message"]?.ToString();
            }

            return response;
        }

        public async Task<string> AssignVehicleAsync(AssignVehicleRequest request)
        {
            // ✅ Convert List<string> → DataTable (for TVP)
            var vehicleTable = new DataTable();
            vehicleTable.Columns.Add("vehicle_no", typeof(string));

            foreach (var vehicle in request.Vehicles)
            {
                vehicleTable.Rows.Add(vehicle);
            }

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", request.UserId),
                //new SqlParameter("@CreatedBy", request.CreatedBy),

                new SqlParameter("@VehicleList", SqlDbType.Structured)
                {
                    TypeName = "dbo.VehicleListType", // MUST match SQL type
                    Value = vehicleTable
                }
            };

            await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "usp_SaveVehicleAssign",
                parameters
            );

            return "Vehicle assigned successfully";
        }


        public async Task<DataTable> GetUserVehicleDataAsync(GetVehicleRequest request)
        {
            // ✅ Convert List<string> → DataTable (for TVP)
            var vehicleTable = new DataTable();
            vehicleTable.Columns.Add("vehicle_no", typeof(string));

            if(request.Vehicles != null)
            {
                foreach (var vehicle in request.Vehicles)
                {
                    vehicleTable.Rows.Add(vehicle);
                }
            }
            // ✅ Convert List<string> → DataTable (for TVP)
            var mobielTables = new DataTable();
            mobielTables.Columns.Add("mobile_no", typeof(string));

            if(request.MobileNo != null)
            {
                foreach (var mobile in request.MobileNo)
                {
                    mobielTables.Rows.Add(mobile);
                }
            }
            // ✅ Convert List<string> → DataTable (for TVP)
            var userNameTable = new DataTable();
            userNameTable.Columns.Add("user_name", typeof(string));

            if (request.UserNames != null)
            {
                foreach (var userName in request.UserNames)
                {
                    userNameTable.Rows.Add(userName);
                }
            }

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", (object?)request.userId ?? DBNull.Value),
                new SqlParameter("@VehicleList", SqlDbType.Structured)
                {
                    TypeName = "dbo.VehicleListType", // MUST match SQL type
                    Value = vehicleTable
                },
                new SqlParameter("@MobileNos", SqlDbType.Structured)
                {
                    TypeName = "dbo.MobileNoList", // MUST match SQL type
                    Value = mobielTables
                },
                new SqlParameter("@UserNames", SqlDbType.Structured)
                {
                    TypeName = "dbo.UserNameList", // MUST match SQL type
                    Value = userNameTable
                },
                new SqlParameter("@SubUserId", (object?)request.subUserId ?? DBNull.Value),
                new SqlParameter("@FromDate", (object?)request.fromDate ?? DBNull.Value),
                new SqlParameter("@ToDate", (object?)request.toDate ?? DBNull.Value),
                new SqlParameter("@AccessType", (object?)request.accessType ?? DBNull.Value)
            };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(_connStr,"usp_GetUserVehicleData",
                parameters
            );

            return result;
        }

        public async Task<DataTable> GetUnassignedVehicleListAsync(GetVehicleRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", (object?)request.userId ?? DBNull.Value),
                new SqlParameter("@AccessType", (object?)request.accessType ?? DBNull.Value),
                new SqlParameter("@Action", "vehicle_list")
            };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "usp_GetVehicleList",
                parameters
            );

            return result;
        }

        public async Task<DataTable> GetAssignedVehicleListAsync(GetVehicleRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", (object?)request.userId ?? DBNull.Value),
                new SqlParameter("@SubUserId", (object?)request.subUserId ?? DBNull.Value),
                new SqlParameter("@AccessType", (object?)request.accessType ?? DBNull.Value),
                new SqlParameter("@Action", "assigned_vehicle_list")
            };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "usp_GetVehicleList",
                parameters
            );

            return result;
        }
    }
}
