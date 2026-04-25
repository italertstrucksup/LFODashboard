using Microsoft.AspNetCore.Http;
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
        Task<ProfileResponse?> GetProfileDetailsByIdAsync(Guid userId);
        Task<ProfileResponse> InsertFleetOperatorDocument(UpdateDocumentRequest request);
        Task<ProfileResponse> InsertPreferredLane(PreferredLaneRequest request);
        Task<ProfileResponse> InsertTruckDetails(TruckDetailsRequest request);
        
        Task<ProfileResponse> InsertFleetOperatorKYC(KYCRequest request);



        Task<CompleteKYCResponse> GetCompleteKYCDataAsync(int userId);



    }
}
