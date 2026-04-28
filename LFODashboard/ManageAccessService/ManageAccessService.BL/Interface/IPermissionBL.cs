using Common.Core;
using ManageAccessService.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManageAccessService.BL.Interface
{
    public interface IPermissionBL
    {
        Task<ApiResponse<string>> GetModules(PermissionRequest request);
        Task<ApiResponse<string>> GetFeatures(PermissionRequest request);
    }
}
