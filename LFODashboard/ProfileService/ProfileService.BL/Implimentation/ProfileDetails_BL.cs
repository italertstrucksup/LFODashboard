using Common.Core;
using ProfileService_LFO.BL.Interface;
using ProfileService_LFO.DAL.Interface;
using ProfileService_LFO.Model.Model;
using System.Data;

public class ProfileDetailsBL : IprofileDetails_BL
{
    private readonly IprofileDetailsDL _dl;

    public ProfileDetailsBL(IprofileDetailsDL dl)
    {
        _dl = dl;
    }



    #region Update Fleet Operator
    public async Task<ApiResponse<ProfileResponse>> UpdateFleetOperator(UpdateProfileRequest request)
    {
        try
        {
            var message = await _dl.UpdateFleetOperator(request);

            if (string.IsNullOrEmpty(message))
                return ApiResponse<ProfileResponse>.FailResponse("Failed to update profile", 404);

            var result = new ProfileResponse
            {
                UserId = (Guid)request.UserId
            };

            return ApiResponse<ProfileResponse>.SuccessResponse(result, "Profile updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProfileResponse>.FailResponse(ex.Message);
        }
    }
    #endregion

    #region Update Fleet Operator Type
    public async Task<ApiResponse<ProfileResponse>> InsertFleetOperatorbyType(UpdateFleetOperatorRequest request)
    {
        try
        {
            var message = await _dl.InsertFleetOperatorbyType(request);

            if (string.IsNullOrEmpty(message))
                return ApiResponse<ProfileResponse>.FailResponse("Failed to update operator type", 404);

            var result = new ProfileResponse
            {
                UserId = (Guid)request.UserId
            };

            return ApiResponse<ProfileResponse>.SuccessResponse(result, "Operator type updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProfileResponse>.FailResponse(ex.Message);
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
                return ApiResponse<ProfileResponse>.FailResponse("Failed to add preferred lanes", 404);

            var result = new ProfileResponse
            {
                UserId = (Guid)request.UserId
            };

            return ApiResponse<ProfileResponse>.SuccessResponse(result, "Preferred lanes added successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProfileResponse>.FailResponse(ex.Message);
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
                return ApiResponse<ProfileResponse>.FailResponse("Failed to upload document", 404);

            var result = new ProfileResponse
            {
                UserId = (Guid)request.UserId
            };

            return ApiResponse<ProfileResponse>.SuccessResponse(
                result,
                "Documents uploaded successfully"
            );
        }
        catch (Exception ex)
        {
            return ApiResponse<ProfileResponse>.FailResponse(ex.Message);
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
                return ApiResponse<ProfileResponse>.FailResponse("Failed to add truck", 404);

            var result = new ProfileResponse
            {
                UserId = (Guid)request.UserId
            };

            return ApiResponse<ProfileResponse>.SuccessResponse(result, "Truck added successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProfileResponse>.FailResponse(ex.Message);
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
                return ApiResponse<ProfileResponse>.FailResponse("Failed to save KYC", 404);

            var result = new ProfileResponse
            {
                UserId = (Guid)request.UserId
            };

            return ApiResponse<ProfileResponse>.SuccessResponse(result, "KYC saved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<ProfileResponse>.FailResponse(ex.Message);
        }
    }
    #endregion

    #region Get Complete KYC Data
    public async Task<ApiResponse<CompleteKYCResponse>> GetCompleteKYCDataAsync(Guid userId)
    {
        try
        {
            var ds = await _dl.GetCompleteKYCDataAsync(userId);

            if (ds == null || ds.Tables.Count == 0)
                return ApiResponse<CompleteKYCResponse>.FailResponse("No data found", 404);

            var response = new CompleteKYCResponse();

            // Operator Details
            if (ds.Tables[0].Rows.Count > 0)
            {
                var row = ds.Tables[0].Rows[0];

                response.OperatorDetails = new OperatorDetails
                {
                    Id = row["id"] == DBNull.Value
                        ? Guid.Empty
                        : (row["id"] is Guid g0
                            ? g0
                            : Guid.Parse(row["id"].ToString())),

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

            // Documents
            if (ds.Tables.Count > 1)
            {
                response.Documents = new List<RegistrationDocument>();

                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    response.Documents.Add(new RegistrationDocument
                    {
                        Id = row["id"] == DBNull.Value
                            ? Guid.Empty
                            : (row["id"] is Guid g2
                                ? g2
                                : Guid.Parse(row["id"].ToString())),

                        DocumentType = row["document_type"]?.ToString(),
                        DocumentUrl = row["document_url"]?.ToString(),
                        DocumentNumber = row["document_number"]?.ToString(),
                        IsVerified = row["is_verified"] != DBNull.Value &&
                                     Convert.ToBoolean(row["is_verified"])
                    });
                }
            }

            // Truck Details
            if (ds.Tables.Count > 2)
            {
                response.TruckDetails = new List<TruckDetail>();

                foreach (DataRow row in ds.Tables[2].Rows)
                {
                    response.TruckDetails.Add(new TruckDetail
                    {
                        Id = row["id"] != DBNull.Value
                            ? (long.TryParse(row["id"].ToString(), out long val4) ? val4 : 0)
                            : 0,

                        VehicleNo = row["vehicle_no"]?.ToString(),
                        OwnershipType = row["ownership_type"]?.ToString(),
                        BodyTypeId = row["body_type_id"] != DBNull.Value ? Convert.ToInt32(row["body_type_id"]) : 0,
                        TyreId = row["tyre_id"] != DBNull.Value ? Convert.ToInt32(row["tyre_id"]) : 0,
                        CapacityId = row["capacity_id"] != DBNull.Value ? Convert.ToInt32(row["capacity_id"]) : 0,
                        SizeId = row["size_id"] != DBNull.Value ? Convert.ToInt32(row["size_id"]) : 0
                    });
                }
            }

            // Preferred Lanes
            if (ds.Tables.Count > 3)
            {
                response.PreferredLanes = new List<PreferredLane>();

                foreach (DataRow row in ds.Tables[3].Rows)
                {
                    response.PreferredLanes.Add(new PreferredLane
                    {
                        Id = row["id"] != DBNull.Value
                            ? (long.TryParse(row["id"].ToString(), out long val5) ? val5 : 0)
                            : 0,

                        MobileNo = row["mobile_no"]?.ToString(),
                        FromLocation = row["from_location"]?.ToString(),
                        ToLocation = row["to_location"]?.ToString(),
                        FromState = row["from_state"]?.ToString(),
                        ToState = row["to_state"]?.ToString()
                    });
                }
            }

            // KYC Details
            if (ds.Tables.Count > 4 && ds.Tables[4].Rows.Count > 0)
            {
                var row = ds.Tables[4].Rows[0];

                response.KYCDetails = new KYCDetails
                {
                    Id = row["id"] != DBNull.Value
                        ? (long.TryParse(row["id"].ToString(), out long valId) ? valId : 0)
                        : 0,

                    KYCType = row["kyc_type"]?.ToString(),
                    KYCNumber = row["kyc_number"]?.ToString(),
                    KYCProfileImage = row["kyc_profile_image"]?.ToString(),
                    KYCDocFront = row["kyc_doc_front"]?.ToString(),
                    KYCDocBack = row["kyc_doc_back"]?.ToString(),
                    IsOTPVerified = row.Table.Columns.Contains("is_otp_verified")
                                    && row["is_otp_verified"] != DBNull.Value
                                    && Convert.ToBoolean(row["is_otp_verified"])
                };
            }

            return ApiResponse<CompleteKYCResponse>.SuccessResponse(
                response,
                "Complete registration data fetched successfully",
                200
            );
        }
        catch (Exception ex)
        {
            return ApiResponse<CompleteKYCResponse>.FailResponse(ex.Message);
        }
    }
    #endregion
}