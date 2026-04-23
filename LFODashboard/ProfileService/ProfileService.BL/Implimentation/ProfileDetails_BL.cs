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
    public async Task<ProfileResponse?> GetProfileDetailsByIdAsync(int userId)
    {
        var dt = await _dl.GetProfileDetailsbyID(userId);

        if (dt == null || dt.Rows.Count == 0)
            return null;

        var row = dt.Rows[0];

        return new ProfileResponse
        {
            UserId = Convert.ToInt32(row["UserId"]),
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
        var success = await _dl.UpdateFleetOperator(request);

        if (!success)
            throw new Exception("Failed to save profile");

        // Fetch updated data
        var profile = await GetProfileDetailsByIdAsync((int)request.UserId);

        if (profile == null)
            throw new Exception("Profile not found after save");

        profile.Message = "Profile saved successfully";

        return profile;
    }
    #endregion

    #region update
    public async Task<ProfileResponse> InsertFleetOperatorbyType(UpdateFleetOperatorRequest request)
    {
        var success = await _dl.InsertFleetOperatorbyType(request);

        if (!success)
            throw new Exception("Failed to save profile");

        // Fetch updated data
        var profile = await GetProfileDetailsByIdAsync((int)request.UserId);

        if (profile == null)
            throw new Exception("Profile not found after save");

        profile.Message = "Profile saved successfully";

        return profile;
    }
    #endregion
    public async Task<bool> InsertFleetOperatorDocument(UpdateDocumentRequest request)
    {
        var result = await _dl.InsertFleetOperatorDocument(request);

        if (!result)
            throw new Exception("Failed to save documents");

        return true;
    }

    public async Task<bool> InsertPreferredLane(PreferredLaneRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var result = await _dl.InsertPreferredLane(request);

        if (!result)
            throw new Exception("Failed to add lane");

        return true;
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