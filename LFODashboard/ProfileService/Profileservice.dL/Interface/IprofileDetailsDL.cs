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
        Task<bool> UpdateFleetOperator(UpdateFleetOperatorRequest request);
        Task<bool> InsertFleetOperatorbyType(UpdateFleetOperatorRequest request);
        Task<bool> InsertTruckDetails(TruckDetailsRequest request);
        Task<bool> InsertFleetOperatorDocument(UpdateDocumentRequest request);

        Task<bool> InsertPreferredLane(PreferredLaneRequest request);  
        Task<DataTable> GetLanesAsync(long loginId);
        Task<bool> InsertFleetOperatorKYC(KYCRequest request);

        Task<DataTable> GetKYCAsync(long profileId);
        Task<bool> UpsertKYCDocumentsAsync(KYCDocumentRequest request);
    }
}
