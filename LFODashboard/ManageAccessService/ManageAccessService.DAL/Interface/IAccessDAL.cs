using ManageAccessService.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ManageAccessService.DAL.Interface
{
    public interface IAccessDAL
    {
        public Task<DataTable> GetRoleAsync();
        public Task<UserApiResponse> AddUserAsync(UserApiRequest request);
        public Task<UserApiResponse> EditUserAsync(UserApiRequest request);
        public Task<UserApiResponse> DeleteUserAsync(UserApiRequest request);
        public Task<string> AssignVehicleAsync(AssignVehicleRequest request);

        public Task<DataTable> GetUserVehicleDataAsync(GetVehicleRequest request);
        public Task<DataTable> GetUnassignedVehicleListAsync(GetVehicleRequest request);
        public Task<DataTable> GetAssignedVehicleListAsync(GetVehicleRequest request);
    }
}
