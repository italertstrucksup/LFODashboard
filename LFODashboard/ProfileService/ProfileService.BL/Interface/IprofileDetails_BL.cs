using ProfileService_LFO.DAL.Interface;
using ProfileService_LFO.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ProfileService_LFO.BL.Interface
{
    public interface IprofileDetails_BL
    {
        Task<ProfileResponse> UpdateFleetOperator(UpdateFleetOperatorRequest request);
        Task<ProfileResponse> InsertFleetOperatorbyType(UpdateFleetOperatorRequest request);
        Task<ProfileResponse?> GetProfileDetailsByIdAsync(int userId);
        Task<bool> InsertFleetOperatorDocument(UpdateDocumentRequest request);
        Task<bool> InsertPreferredLane(PreferredLaneRequest request);
        Task<DataTable> GetLanesAsync(long loginId);

        //Task<bool> AddTruckAsync(TruckDetailsRequest request);
        //Task<DataTable> GetTrucksAsync(long profileId);
        Task<bool> InsertFleetOperatorKYC(KYCRequest request);
        Task<DataTable> GetKYCAsync(long profileId);
        Task<bool> UpsertKYCDocumentsAsync(KYCDocumentRequest request);



    }
}
