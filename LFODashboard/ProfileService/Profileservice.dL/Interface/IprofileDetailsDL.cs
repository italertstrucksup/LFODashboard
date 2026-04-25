using ProfileService_LFO.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ProfileService_LFO.DAL.Interface
{
    public interface IprofileDetailsDL
    {
        Task<DataTable> GetProfileDetailsbyID(Guid userId);
        Task<(bool IsSuccess, string Message)> UpdateFleetOperator(UpdateFleetOperatorRequest request);
        Task<(bool IsSuccess, string Message)> InsertFleetOperatorbyType(UpdateFleetOperatorRequest request);
        Task<(bool IsSuccess, string Message)> InsertTruckDetails(TruckDetailsRequest request);
        Task<(bool IsSuccess, string Message)> InsertFleetOperatorDocument(UpdateDocumentRequest request);
        Task<(bool IsSuccess, string Message)> InsertPreferredLane(PreferredLaneRequest request);
        Task<(bool IsSuccess, string Message)> InsertFleetOperatorKYC(KYCRequest request);
    }
}
