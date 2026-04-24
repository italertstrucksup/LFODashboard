using HttpClientLib;
using Microsoft.AspNetCore.Http;
using ProfileService_LFO.BL.Interface;
using ProfileService_LFO.DAL.Implimentation;
using ProfileService_LFO.DAL.Interface;
using ProfileService_LFO.Model.Model;
using System.ComponentModel.Design;
using System.Data;
using System.Text.Json;
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

    public async Task<ProfileResponse> InsertTruckDetails(TruckDetailsRequest request)
    {
       
        var result = await _dl.InsertTruckDetails(request);

        return new ProfileResponse
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            UserId = (Guid)request.UserId
        };
    }

    public async Task<ProfileResponse> InsertFleetOperatorKYC(KYCRequest request)
    {
        var result = await _dl.InsertFleetOperatorKYC(request);

        return new ProfileResponse
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            UserId = (Guid)request.UserId
        };
    }
    //public async Task<ProfileResponse> InsertFleetOperatorDocument(
    //UpdateDocumentRequest request,
    //IFormFile file)
    //{
    //    if (file == null)
    //        throw new Exception("File is required");

    //    // 🔹 Convert file → Base64
    //    using var ms = new MemoryStream();
    //    await file.CopyToAsync(ms);
    //    var base64 = Convert.ToBase64String(ms.ToArray());

    //    // 🔹 Prepare Media API request
    //    var mediaRequest = new
    //    {
    //        Data = new
    //        {
    //            File = base64,
    //            FolderName = "KYC"
    //        }
    //    };

    //    // 🔹 Call Media API
    //    var response = await _httpService.PostAsync<object, ApiResponse<object>>(
    //        "https://localhost:7235/api/Media/upload-base64", // 🔥 FIX PORT
    //        mediaRequest
    //    );

    //    if (response == null || !response.Success)
    //        throw new Exception(response?.Message ?? "Upload failed");

    //    // 🔹 Extract documentKey
    //    var json = JsonSerializer.Serialize(response.Data);
    //    var doc = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

    //    if (doc == null || !doc.ContainsKey("documentKey"))
    //        throw new Exception("Invalid response from Media API");

    //    string documentKey = doc["documentKey"];

    //    // 🔹 Save in DB
    //    request.DocumentUrl = documentKey;

    //    var result = await _dl.InsertFleetOperatorDocument(request);

    //    if (!result.IsSuccess)
    //        throw new Exception(result.Message);

    //    return new ProfileResponse
    //    {
    //        IsSuccess = true,
    //        Message = result.Message
    //    };
    //}










}