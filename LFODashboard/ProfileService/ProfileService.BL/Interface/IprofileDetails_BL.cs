using Common.Core;
using ProfileService_LFO.Model.Model;

namespace ProfileService_LFO.BL.Interface
{
    public interface IprofileDetails_BL
    {
        Task<ApiResponse<ProfileResponse>> UpdateFleetOperator(UpdateProfileRequest request);

        Task<ApiResponse<ProfileResponse>> InsertFleetOperatorbyType(UpdateFleetOperatorRequest request);


        Task<ApiResponse<ProfileResponse>> InsertFleetOperatorDocument(UpdateDocumentRequest request);

        Task<ApiResponse<ProfileResponse>> InsertPreferredLane(PreferredLaneRequest request);

        Task<ApiResponse<ProfileResponse>> InsertTruckDetails(TruckDetailsRequest request);

        Task<ApiResponse<ProfileResponse>> InsertFleetOperatorKYC(KYCRequest request);
        Task<ApiResponse<CompleteKYCResponse>> GetCompleteKYCDataAsync(Guid userId);

    }
}