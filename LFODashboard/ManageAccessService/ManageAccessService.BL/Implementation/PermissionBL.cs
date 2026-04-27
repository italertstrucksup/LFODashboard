using Common.Core;
using ManageAccessService.BL.Interface;
using ManageAccessService.DAL.Interface;
using ManageAccessService.Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ManageAccessService.BL.Implementation
{
    public class PermissionBL : IPermissionBL
    {
        private readonly IPermissionDAL _permissionDAL;
        public PermissionBL(IPermissionDAL permissionDAL)
        {
            _permissionDAL = permissionDAL;
        }
        public async Task<ApiResponse<string>> GetModules(PermissionRequest request)
        {
            try
            {
                if (request == null)
                    return ApiResponse<string>.FailResponse("Invalid request", 400);

                var result = await _permissionDAL.GetModules(request);

                if (result == null || result.Rows.Count == 0)
                    return ApiResponse<string>.FailResponse("No data found", 404);


                var groupedData = result.AsEnumerable()
                    .GroupBy(row => new
                    {
                        module_id = row.Field<int>("module_id"),
                        module_name = row.Field<string>("module_name")
                    })
                    .Select(g => new
                    {
                        module_id = g.Key.module_id,
                        module_name = g.Key.module_name,
                        sub_modules = g
                            .Where(x => x["id"] != DBNull.Value)
                            .Select(x => new
                            {
                                id = x.Field<int>("id"),
                                sub_module_name = x.Field<string>("sub_module_name")
                            })
                            .ToList()
                    })
                    .ToList();

                return ApiResponse<string>.SuccessResponse(JsonConvert.SerializeObject(groupedData), "Data fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message);
            }
        }

        public async Task<ApiResponse<string>> GetFeatures(PermissionRequest request)
        {
            try
            {
                if (request == null)
                    return ApiResponse<string>.FailResponse("Invalid request", 400);

                var result = await _permissionDAL.GetFeatures(request);

                if (result == null || result.Rows.Count == 0)
                    return ApiResponse<string>.FailResponse("No data found", 404);

                return ApiResponse<string>.SuccessResponse(JsonConvert.SerializeObject(result), "Data fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message);
            }
        }
    }
}
