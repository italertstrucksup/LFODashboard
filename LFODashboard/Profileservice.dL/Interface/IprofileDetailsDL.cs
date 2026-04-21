using ProfileService_LFO.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ProfileService_LFO.DAL.Interface
{
    public interface IprofileDetailsDL
    {
        Task<DataTable> GetProfileDetailsbyID(int userId);
        Task<bool> UpsertProfileAsync(UpsertProfileRequest request);
        Task<bool> UpsertDocumentsAsync(UpsertDocumentRequest request);

        Task<bool> InsertLaneAsync(PreferredLaneRequest request);
        Task<DataTable> GetLanesAsync(long loginId);
        Task<bool> UpsertKYCAsync(KYCRequest request);

        Task<DataTable> GetKYCAsync(long profileId);
        Task<bool> UpsertKYCDocumentsAsync(KYCDocumentRequest request);
    }
}
