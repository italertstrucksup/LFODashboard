using ProfileService_LFO.DAL.Interface;
using ProfileService_LFO.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ProfileService_LFO.BL.Interface
{
    public interface IprofileDetails_BL
    {
        Task<ProfileResponse> UpsertProfileAsync(UpsertProfileRequest request);
        Task<ProfileResponse?> GetProfileDetailsByIdAsync(int userId);
        Task<bool> UpsertDocumentsAsync(UpsertDocumentRequest request);
        Task<bool> AddLaneAsync(PreferredLaneRequest request);
        Task<DataTable> GetLanesAsync(long loginId);

        Task<bool> AddTruckAsync(TruckDetailsRequest request);
        Task<DataTable> GetTrucksAsync(long profileId);
        Task<bool> UpsertKYCAsync(KYCRequest request);
        Task<DataTable> GetKYCAsync(long profileId);
        Task<bool> UpsertKYCDocumentsAsync(KYCDocumentRequest request);



    }
}
