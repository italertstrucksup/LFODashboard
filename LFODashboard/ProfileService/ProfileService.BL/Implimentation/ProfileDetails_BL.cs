using ProfileService_LFO.BL.Interface;
using ProfileService_LFO.DAL.Implimentation;
using ProfileService_LFO.DAL.Interface;
using ProfileService_LFO.Model.Model;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class ProfileDetailsBL : IprofileDetails_BL
{
    private readonly IprofileDetailsDL _dl;

    public ProfileDetailsBL(IprofileDetailsDL dl)
    {
        _dl = dl;
    }

    #region Get By Id
    public async Task<ProfileResponse?> GetProfileDetailsByIdAsync(Guid userId)
    {
        var dt = await _dl.GetProfileDetailsbyID(userId);

        if (dt == null || dt.Rows.Count == 0)
            return null;

        var row = dt.Rows[0];

        return new ProfileResponse
        {
            UserId = row["UserId"] == DBNull.Value ? Guid.Empty : (row["UserId"] is Guid g ? g : Guid.Parse(row["UserId"].ToString())),
            ProfileName = row["ProfileName"]?.ToString(),
            MobileNo = row["MobileNo"]?.ToString(),
            CompanyName = row["CompanyName"]?.ToString(),
            City = row["City"]?.ToString(),
            State = row["State"]?.ToString(),
            IsKYCDone = row["IsKYCDone"].ToString()
        };
    }
    #endregion

    #region update
    public async Task<ProfileResponse> UpdateFleetOperator(UpdateFleetOperatorRequest request)
    {
        if (request == null || request.UserId == Guid.Empty)
            throw new ArgumentException("UserId is required for update", nameof(request.UserId));

        var result = await _dl.UpdateFleetOperator(request);

        if (!result.IsSuccess)
            throw new Exception(result.Message);

        return new ProfileResponse
        {
            IsSuccess = true,
            Message = result.Message
        };
    }
    #endregion

    #region update
    public async Task<ProfileResponse> InsertFleetOperatorbyType(UpdateFleetOperatorRequest request)
    {
        if (request == null || request.UserId == Guid.Empty)
            throw new ArgumentException("UserId is required");

        var result = await _dl.InsertFleetOperatorbyType(request);

        return new ProfileResponse
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            UserId =  (Guid)request.UserId
        };
    }
    #endregion

    public async Task<ProfileResponse> InsertPreferredLane(PreferredLaneRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var result = await _dl.InsertPreferredLane(request);

        return new ProfileResponse
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            UserId = (Guid)request.UserId
        };
    }
    public async Task<ProfileResponse> InsertFleetOperatorDocument(UpdateDocumentRequest request)
    {
        var result = await _dl.InsertFleetOperatorDocument(request);

        return new ProfileResponse
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            UserId = (Guid)request.UserId
        };
    }

   

    public async Task<DataTable> GetLanesAsync(long loginId)
    {
        return await _dl.GetLanesAsync(loginId);
    }
    public async Task<bool> InsertTruckDetails(TruckDetailsRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        var result = await _dl.InsertTruckDetails(request);

        if (!result)
            throw new Exception("Failed to add truck");

        return true;
    }

    //public async Task<DataTable> GetTrucksAsync(long profileId)
    //{
    //    return await _dl.GetTrucksByProfileId(profileId);
    //}

    public async Task<bool> InsertFleetOperatorKYC(KYCRequest request)
    {
        var result = await _dl.InsertFleetOperatorKYC(request);

        if (!result)
            throw new Exception("Failed to save KYC");

        return true;
    }

    public async Task<DataTable> GetKYCAsync(long profileId)
    {
        return await _dl.GetKYCAsync(profileId);
    }

    public async Task<bool> UpsertKYCDocumentsAsync(KYCDocumentRequest request)
    {
        var result = await _dl.UpsertKYCDocumentsAsync(request);

        if (!result)
            throw new Exception("Failed to save KYC documents");

        return true;
    }


}