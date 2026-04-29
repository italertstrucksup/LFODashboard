using Common.Core;
using MasterAPIServiceModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasterAPIServiceBL.Interface
{
    public interface IMasterBL
    {

        Task<ApiResponse<OperatorMasterResponse>> GetOperatorMaster(int id);
        Task<ApiResponse<DocumentMasterResponse>> GetDocumentMaster(int id);
        Task<ApiResponse<List<VehicleDetailsMasterResponse>>> GetVehicleDetailsMaster(string actionType, int? bodyId, int? tyreId);
        Task<ApiResponse<List<GetLocationByPincodeResponse>>> GetLocationByPincode(string pincode);
        Task<ApiResponse<List<CityMasterResponse>>> GetCityMaster(string city);
        Task<ApiResponse<KYCMasterResponse>> GetKYCMaster(int id);
    }
}
