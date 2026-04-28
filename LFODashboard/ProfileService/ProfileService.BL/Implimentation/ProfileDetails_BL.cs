using HttpClientLib;
using Microsoft.AspNetCore.Http;
using ProfileService_LFO.BL.Interface;
using ProfileService_LFO.DAL.Implimentation;
using ProfileService_LFO.DAL.Interface;
using ProfileService_LFO.Model.Model;
using System.ComponentModel.Design;
using System.Data;
using System.Text.Json;
using System.Collections.Generic;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Common.Core;

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
            UserId = row["user_id"] == DBNull.Value ? Guid.Empty : (row["user_id"] is Guid g ? g : Guid.Parse(row["user_id"].ToString())),
            ProfileName = row["owner_name"]?.ToString(),
            MobileNo = row["mobile_no"]?.ToString(),
            CompanyName = row["company_name"]?.ToString(),
            City = row["city"]?.ToString(),
            State = row["state"]?.ToString(),
            IsKYCDone = row["kyc_status"]?.ToString() ?? "0"
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
            UserId = (Guid)request.UserId
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


    public async Task<ApiResponse<CompleteKYCResponse>> GetCompleteKYCDataAsync(Guid userId)
    {
        var ds = await _dl.GetCompleteKYCDataAsync(userId);
        var response = new CompleteKYCResponse();

        if (ds != null && ds.Tables.Count > 0)
        {
            //operator & addressdetails
            if (ds.Tables[0].Rows.Count > 0)
            {
                var row = ds.Tables[0].Rows[0];
                response.OperatorDetails = new OperatorDetails
                {
                    Id = row["id"] == DBNull.Value ? Guid.Empty : (row["id"] is Guid g0 ? g0 : Guid.Parse(row["id"].ToString())),
                    OwnerName = row["owner_name"]?.ToString(),
                    OperatorType = row["operator_type"]?.ToString(),
                    ProfileImage = row["profile_image"]?.ToString(),
                    KYCStatus = row["kyc_status"]?.ToString(),
                    CompanyName = row["company_name"]?.ToString(),
                    CompanyAddress = row["company_address"]?.ToString(),
                    PinCode = row["pin_code"]?.ToString(),
                    City = row["city"]?.ToString(),
                    State = row["state"]?.ToString(),
                    SubCity = row["sub_city"]?.ToString(),
                    CIN = row["cin"]?.ToString(),
                    GSTNumber = row["gst_number"]?.ToString(),
                    PANNumber = row["pan_number"]?.ToString()
                };
            }

            // documents
            if (ds.Tables.Count > 1)
            {
                response.Documents = new List<RegistrationDocument>();
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    response.Documents.Add(new RegistrationDocument
                    {
                        Id = row["id"] == DBNull.Value ? Guid.Empty : (row["id"] is Guid g2 ? g2 : Guid.Parse(row["id"].ToString())),
                        DocumentType = row["document_type"]?.ToString(),
                        DocumentUrl = row["document_url"]?.ToString(),
                        DocumentNumber = row["document_number"]?.ToString(),
                        IsVerified = row["is_verified"] != DBNull.Value && Convert.ToBoolean(row["is_verified"])
                    });
                }
            }

            //truck details
            if (ds.Tables.Count > 2)
            {
                response.TruckDetails = new List<TruckDetail>();
                foreach (DataRow row in ds.Tables[2].Rows)
                {
                    response.TruckDetails.Add(new TruckDetail
                    {
                        Id = row["id"] != DBNull.Value ? (long.TryParse(row["id"].ToString(), out long val4) ? val4 : 0) : 0,
                        VehicleNo = row["vehicle_no"]?.ToString(),
                        OwnershipType = row["ownership_type"]?.ToString(),
                        BodyTypeId = row["body_type_id"] != DBNull.Value ? Convert.ToInt32(row["body_type_id"]) : 0,
                        TyreId = row["tyre_id"] != DBNull.Value ? Convert.ToInt32(row["tyre_id"]) : 0,
                        CapacityId = row["capacity_id"] != DBNull.Value ? Convert.ToInt32(row["capacity_id"]) : 0,
                        SizeId = row["size_id"] != DBNull.Value ? Convert.ToInt32(row["size_id"]) : 0
                    });
                }
            }

            //preferred lanes
            if (ds.Tables.Count > 3)
            {
                response.PreferredLanes = new List<PreferredLane>();
                foreach (DataRow row in ds.Tables[3].Rows)
                {
                    response.PreferredLanes.Add(new PreferredLane
                    {
                        Id = row["id"] != DBNull.Value ? (long.TryParse(row["id"].ToString(), out long val5) ? val5 : 0) : 0,
                        MobileNo = row["mobile_no"]?.ToString(),
                        FromLocation = row["from_location"]?.ToString(),
                        ToLocation = row["to_location"]?.ToString(),
                        FromState = row["from_state"]?.ToString(),
                        ToState = row["to_state"]?.ToString()
                    });
                }
            }

            //kyc details
            if (ds.Tables.Count > 4 && ds.Tables[4].Rows.Count > 0)
            {
                var row = ds.Tables[4].Rows[0];
                response.KYCDetails = new KYCDetails
                {
                    Id = row["id"] != DBNull.Value ? (long.TryParse(row["id"].ToString(), out long valId) ? valId : 0) : 0,
                    KYCType = row["kyc_type"]?.ToString(),
                    KYCNumber = row["kyc_number"]?.ToString(),
                    KYCProfileImage = row["kyc_profile_image"]?.ToString(),
                    KYCDocFront = row["kyc_doc_front"]?.ToString(),
                    KYCDocBack = row["kyc_doc_back"]?.ToString(),
                    IsOTPVerified = row.Table.Columns.Contains("is_otp_verified") && row["is_otp_verified"] != DBNull.Value && Convert.ToBoolean(row["is_otp_verified"])
                };
            }
        }

        return ApiResponse<CompleteKYCResponse>.SuccessResponse(response, "Complete registration data fetched successfully", 200);
    }


}