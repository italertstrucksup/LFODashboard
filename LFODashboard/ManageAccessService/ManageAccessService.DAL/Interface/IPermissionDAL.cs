using ManageAccessService.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ManageAccessService.DAL.Interface
{
    public interface IPermissionDAL
    {
        public Task<DataTable> GetModules(PermissionRequest request);
        public Task<DataTable> GetFeatures(PermissionRequest request);
    }
}
