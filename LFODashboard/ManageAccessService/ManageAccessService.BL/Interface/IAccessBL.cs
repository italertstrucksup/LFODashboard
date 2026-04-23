using Common.Core;
using ManageAccessService.Model.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ManageAccessService.BL.Interface
{
    public interface IAccessBL
    {
        Task<ApiResponse<UserApiResponse>> AddUserAsync(UserApiRequest request);
        Task<ApiResponse<UserApiResponse>> EditUserAsync(UserApiRequest request);
        Task<ApiResponse<string>> AssignVehicleAsync(AssignVehicleRequest request);
        Task<ApiResponse<string>> GetUserVehicleDataAsync(GetVehicleRequest request);
        Task<ApiResponse<string>> GetVehicleListAsync(GetVehicleRequest request);

    }
}
