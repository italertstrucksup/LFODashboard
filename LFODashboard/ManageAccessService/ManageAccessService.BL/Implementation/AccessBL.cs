using Common.Core;
using ManageAccessService.BL.Interface;
using ManageAccessService.DAL.Interface;
using ManageAccessService.Model.Models;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ManageAccessService.BL.Implementation
{
    public class AccessBL : IAccessBL
    {
       private readonly IAccessDAL _accessDAL;
        public AccessBL(IAccessDAL accessDAL)
        {
            _accessDAL = accessDAL;
        }

        public async Task<ApiResponse<UserApiResponse>> AddUserAsync(UserApiRequest request)
        {
            try
            {
                var result = await _accessDAL.AddUserAsync(request);
                if (result == null || string.IsNullOrEmpty(result.UserId))
                    return ApiResponse<UserApiResponse>.FailResponse("Failed to add user", 404);
                else
                {
                    AssignVehicleRequest assignVehicleRequest = new AssignVehicleRequest();
                    assignVehicleRequest.UserId = result.UserId;
                    assignVehicleRequest.Vehicles = request.Vehicles ?? new List<string>();

                    await _accessDAL.AssignVehicleAsync(assignVehicleRequest);
                }
                return ApiResponse<UserApiResponse>.SuccessResponse(result, "User added successfully");
            }
            catch (Exception ex) {
                return ApiResponse<UserApiResponse>.FailResponse(ex.Message, 400);
            }
            
        }

        public async Task<ApiResponse<UserApiResponse>> EditUserAsync(UserApiRequest request)
        {
            try
            {
                var result = await _accessDAL.EditUserAsync(request);
                if (result == null || string.IsNullOrEmpty(result.UserId))
                    return ApiResponse<UserApiResponse>.FailResponse(result.Message, 404);
                else
                {
                    AssignVehicleRequest assignVehicleRequest = new AssignVehicleRequest();
                    assignVehicleRequest.UserId = result.UserId;
                    assignVehicleRequest.Vehicles = request.Vehicles ?? new List<string>();

                    await _accessDAL.AssignVehicleAsync(assignVehicleRequest);
                }
                return ApiResponse<UserApiResponse>.SuccessResponse(result, "User updated successfully");
            }
            catch (Exception ex) {
                return ApiResponse<UserApiResponse>.FailResponse(ex.Message);
            }
            
        }

        public async Task<ApiResponse<string>> AssignVehicleAsync(AssignVehicleRequest request)
        {
            try
            {
                var result = await _accessDAL.AssignVehicleAsync(request);
                if (string.IsNullOrEmpty(result))
                    return ApiResponse<string>.FailResponse("Failed to assign vehicle", 404);

                return ApiResponse<string>.SuccessResponse(result, "Vehicle assigned successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message);
            }
        }

        public async Task<ApiResponse<string>> GetUserVehicleDataAsync(GetVehicleRequest request)
        {
            try
            {
                if (request == null)
                    return ApiResponse<string>.FailResponse("Invalid request", 400);

                var result = await _accessDAL.GetUserVehicleDataAsync(request);

                if (result == null || result.Rows.Count == 0)
                    return ApiResponse<string>.FailResponse("No data found", 404);

                return ApiResponse<string>.SuccessResponse(JsonConvert.SerializeObject(result), "Data fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message);
            }
        }

        public async Task<ApiResponse<string>> GetVehicleListAsync(GetVehicleRequest request)
        {
            try
            {
                if (request == null)
                    return ApiResponse<string>.FailResponse("Invalid request", 400);

                var result = await _accessDAL.GetVehicleListAsync(request);

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
