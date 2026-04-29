using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MasterAPIServiceDAL.Interface
{
    public interface IMasterDAL
    {
        Task<DataTable> GetOperatorMaster(int Id);
        Task<DataTable> GetDocumentMaster(int Id);
        Task<DataTable> GetVehicleDetailsMaster(string Action, int? BodyId, int? TyreId);
        Task<DataTable> GetLocationByPincode(string Pincode);
        Task<DataTable> GetCityMaster(string city);
        Task<DataTable> GetKYCMaster(int Id);

    }

}
