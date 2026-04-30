using Common.Core;
using DashboardService.BL.Interface;
using DashboardService.DAL.Implementation;
using DashboardService.DAL.Interface;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DashboardService.BL.Implementation
{
    public class DashboardBL : IDashboardBL
    {
        private readonly IDashboardDAL _dashboardDAL;

        public DashboardBL(IDashboardDAL dashboardDAL)
        {
            _dashboardDAL = dashboardDAL;
        }

        public async Task<ApiResponse<object>> GetDashboardDataAsync()
        {
            try
            {
                var result = await _dashboardDAL.GetDashboardDataAsync();

                if (result == null || result.Rows.Count == 0)
                {
                    return ApiResponse<object>.FailResponse("No data found", 404);
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (DataRow row in result.Rows)
                {
                    sb.Append(row[0].ToString());
                }

                string rawJson = sb.ToString();
                object jsonData = null;

                if (!string.IsNullOrWhiteSpace(rawJson))
                {
                    jsonData = System.Text.Json.JsonSerializer.Deserialize<object>(rawJson);
                }

                return ApiResponse<object>.SuccessResponse(jsonData, "Dashboard data fetched successfully");
            }
            catch (Exception ex)
            {
                // Log exception here if logger is available
                return ApiResponse<object>.FailResponse(ex.Message, 500);
            }
        }
    }
}
