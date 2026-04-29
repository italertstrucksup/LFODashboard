using ProfileService_LFO.Model.Model;
using System.Data;

namespace ProfileService_LFO.DAL.Interface
{
    public interface IprofileDetailsDL
    {
        Task<DataTable> GetProfileDetailsbyID(Guid userId);

        Task<DataSet> GetCompleteKYCDataAsync(Guid userId);

        Task<string> UpdateFleetOperator(UpdateProfileRequest request);

        Task<string> InsertFleetOperatorbyType(UpdateFleetOperatorRequest request);

        Task<string> InsertTruckDetails(TruckDetailsRequest request);
        Task<string> InsertFleetOperatorDocument(UpdateDocumentRequest request);
        Task<string> InsertPreferredLane(PreferredLaneRequest request);
        Task<string> InsertFleetOperatorKYC(KYCRequest request);
    }
}