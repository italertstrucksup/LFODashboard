using ProfileService_LFO.BL.Interface;
using ProfileService_LFO.DAL.Interface;
using ProfileService_LFO.Model.Model;
using Common.Core;

public class ProfileDetailsBL : IprofileDetails_BL
{
    private readonly IprofileDetailsDL _dl;

    public ProfileDetailsBL(IprofileDetailsDL dl)
    {
        _dl = dl;
    }

    #region Update Fleet Operator
    public async Task<ApiResponse<ProfileResponse>> UpdateFleetOperator(UpdateFleetOperatorRequest request)
    {
        try
        {
            var message = await _dl.UpdateFleetOperator(request);

            if (string.IsNullOrEmpty(message))
                return ApiResponse<ProfileResponse>.FailResponse("Failed to update fleet operator", 404);
           

            var result = new ProfileResponse
            {
                UserId = request.UserId.ToString(),

            };

            return ApiResponse<ProfileResponse>.SuccessResponse(result, "Fleet operator updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProfileResponse>.FailResponse(ex.Message, 400);
        }
    }
    #endregion


    #region Insert Fleet Operator By Type
    public async Task<ApiResponse<ProfileResponse>> InsertFleetOperatorbyType(UpdateFleetOperatorRequest request)
    {
        try
        {
            var message = await _dl.InsertFleetOperatorbyType(request);

            if (string.IsNullOrEmpty(message))
                return ApiResponse<ProfileResponse>.FailResponse("Failed to insert operator type", 404);
            

            var result = new ProfileResponse
            {
                UserId = request.UserId.ToString(),

            };

            return ApiResponse<ProfileResponse>.SuccessResponse(result, "Operator type inserted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProfileResponse>.FailResponse(ex.Message, 400);
        }
    }
    #endregion


    #region Insert Preferred Lane
    public async Task<ApiResponse<ProfileResponse>> InsertPreferredLane(PreferredLaneRequest request)
    {
        try
        {
            var message = await _dl.InsertPreferredLane(request);

            if (string.IsNullOrEmpty(message))
                return ApiResponse<ProfileResponse>.FailResponse("Failed to insert preferred lane", 404);
           

            var result = new ProfileResponse
            {
                UserId = request.UserId.ToString(),

            };

            return ApiResponse<ProfileResponse>.SuccessResponse(result, "Preferred lane inserted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProfileResponse>.FailResponse(ex.Message, 400);
        }
    }
    #endregion


    #region Insert Fleet Operator Document
    public async Task<ApiResponse<ProfileResponse>> InsertFleetOperatorDocument(UpdateDocumentRequest request)
    {
        try
        {
            var message = await _dl.InsertFleetOperatorDocument(request);

            if (string.IsNullOrEmpty(message))
                return ApiResponse<ProfileResponse>.FailResponse("Failed to insert document", 404);
            else
            {
                // same structure
            }

            var result = new ProfileResponse
            {
                UserId = request.UserId.ToString(),

            };

            return ApiResponse<ProfileResponse>.SuccessResponse(result, "Document inserted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProfileResponse>.FailResponse(ex.Message, 400);
        }
    }
    #endregion


    #region Insert Truck Details
    public async Task<ApiResponse<ProfileResponse>> InsertTruckDetails(TruckDetailsRequest request)
    {
        try
        {
            var message = await _dl.InsertTruckDetails(request);

            if (string.IsNullOrEmpty(message))
                return ApiResponse<ProfileResponse>.FailResponse("Failed to insert truck details", 404);
            else
            {
                // same structure
            }

            var result = new ProfileResponse
            {
                UserId = request.UserId.ToString(),

            };

            return ApiResponse<ProfileResponse>.SuccessResponse(result, "Truck details inserted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProfileResponse>.FailResponse(ex.Message, 400);
        }
    }
    #endregion


    #region Insert Fleet Operator KYC
    public async Task<ApiResponse<ProfileResponse>> InsertFleetOperatorKYC(KYCRequest request)
    {
        try
        {
            var message = await _dl.InsertFleetOperatorKYC(request);

            if (string.IsNullOrEmpty(message))
                return ApiResponse<ProfileResponse>.FailResponse("Failed to insert KYC", 404);
           

            var result = new ProfileResponse
            {
                UserId = request.UserId.ToString(),
            };

            return ApiResponse<ProfileResponse>.SuccessResponse(result, "KYC inserted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProfileResponse>.FailResponse(ex.Message, 400);
        }
    }
    #endregion
}