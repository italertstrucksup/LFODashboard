using Common.Core;
using HttpClientLib;
using Microsoft.Extensions.Configuration;
using ProfilePhotoService.BL.Interface;
using ProfilePhotoService.DAL.Interface;
using ProfilePhotoService.Model.Models;
using System.Data;

namespace ProfilePhotoService.BL.Implemetation
{
    public class ProfilePhotoBL : IProfilePhotoBL
    {
        private readonly IProfilePhotoDAL _profilePhotoDAL;
        private readonly IHttpService _httpService;
        private readonly IConfiguration _config;

        public ProfilePhotoBL(IProfilePhotoDAL profilePhotoDAL, IHttpService httpService, IConfiguration config)
        {
            _profilePhotoDAL = profilePhotoDAL;
            _httpService = httpService;
            _config = config;
        }

        public async Task<ApiResponse<string>> SendUploadLinkAsync(UploadLinkRequest request)
        {
            try
            {
                string token = Guid.NewGuid().ToString("N");

                var result = await _profilePhotoDAL.SaveUploadLinkAsync(request.LoginId, request.MobileNo, token);

                if (result.Rows.Count > 0 && result.Rows[0]["Result"].ToString() == "SUCCESS")
                {
                    string baseUrl = _config["AppConfig:MobileUploadUrl"] ?? "https://localhost:7052/capture.html";
                    string link = $"{baseUrl}?token={token}&loginId={request.LoginId}&PhotoType={request.PhotoType}";

                    string template = request.PhotoType.ToLower() == "live"
                        ? $"Hi, please click the link to capture your LIVE photo: {link}. This link is valid for one-time use only."
                        : $"Hi, please click the link to upload your PROFILE photo: {link}. This link is valid for one-time use only.";

                    var smsResponse = await CallSmsApi(request.MobileNo, template);

                    if (smsResponse)
                    {
                        return ApiResponse<string>.SuccessResponse(link, $"{request.PhotoType} photo link sent successfully");
                    }
                    return ApiResponse<string>.FailResponse("Failed to send link", 500);
                }

                return ApiResponse<string>.FailResponse(result.Rows[0]["Message"]?.ToString() ?? "Error generating link", 500);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> ResendUploadLinkAsync(UploadLinkRequest request)
        {
            return await SendUploadLinkAsync(request);
        }

        public async Task<ApiResponse<bool>> ValidateTokenAsync(string token)
        {
            try
            {
                var result = await _profilePhotoDAL.ValidateUploadTokenAsync(token);
                if (result.Rows.Count > 0 && result.Rows[0]["Result"].ToString() == "SUCCESS")
                {
                    return ApiResponse<bool>.SuccessResponse(true, "Token valid");
                }
                return ApiResponse<bool>.FailResponse("Invalid or used link", 401);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> UploadPhotoAsync(PhotoUploadRequest request)
        {
            try
            {
                var validationResult = await _profilePhotoDAL.ValidateUploadTokenAsync(request.Token);
                if (validationResult.Rows.Count == 0 || validationResult.Rows[0]["Result"].ToString() != "SUCCESS")
                {
                    return ApiResponse<bool>.FailResponse("Invalid or used link.", 401);
                }

                Guid userId = Guid.Parse(validationResult.Rows[0]["UserId"].ToString()!);

                string photoType = validationResult.Columns.Contains("photo_type")
                                   ? validationResult.Rows[0]["photo_type"].ToString()!
                                   : "profile";
            
                string mediaServiceUrl = _config["AppConfig:MediaServiceUrl"] ?? "https://localhost:7126/api/Media";

                var apiUploadRequest = new ApiRequest<MediaUploadRequest>
                {
                    Data = new MediaUploadRequest
                    {
                        File = request.ImageBase64,
                        FolderName = "user"
                    },
                    Requestedby = "ProfilePhotoService"
                };

                var uploadResponse = await _httpService.PostAsync<ApiRequest<MediaUploadRequest>, ApiResponse<object>>($"{mediaServiceUrl}/upload-base64", apiUploadRequest);

                if (uploadResponse != null && uploadResponse.Success)
                {
                    string documentKey = "";
                    if (uploadResponse.Data is System.Text.Json.JsonElement element && element.TryGetProperty("documentKey", out var prop))
                    {
                        documentKey = prop.GetString() ?? "";
                    }

                    var updateResult = await _profilePhotoDAL.UpdateProfilePhotoAsync(userId, documentKey, photoType);

                    if (updateResult.Rows.Count > 0 && updateResult.Rows[0]["Result"].ToString() == "SUCCESS")
                    {
                        await _profilePhotoDAL.ExpireTokenAsync(request.Token);
                        return ApiResponse<bool>.SuccessResponse(true, "Profile photo synced successfully.");
                    }

                    string dbError = (updateResult.Rows.Count > 0) ? updateResult.Rows[0]["Message"]?.ToString() : "Database update failed (No response from SP)";
                    return ApiResponse<bool>.FailResponse(dbError, 500);
                }

                string remoteError = uploadResponse?.Message ?? "Unreachable";
                return ApiResponse<bool>.FailResponse($"Media Service Error: {remoteError}", 500);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<UploadStatusResponse>> GetUploadStatusAsync(Guid loginId)
        {
            try
            {
               
                var result = await _profilePhotoDAL.GetUploadStatusAsync(loginId, "profile");
                if (result.Rows.Count > 0)
                {
                    var response = new UploadStatusResponse
                    {
                        IsUploaded = !string.IsNullOrEmpty(result.Rows[0]["PhotoPath"]?.ToString()),
                        ImageUrl = result.Rows[0]["PhotoPath"]?.ToString(),
                        StatusMessage = !string.IsNullOrEmpty(result.Rows[0]["PhotoPath"]?.ToString()) ? "Image Uploaded Successfully" : "Awaiting upload...",
                        UploadedAt = result.Rows[0]["UpdatedAt"] != DBNull.Value ? Convert.ToDateTime(result.Rows[0]["UpdatedAt"]) : null
                    };
                    return ApiResponse<UploadStatusResponse>.SuccessResponse(response);
                }
                return ApiResponse<UploadStatusResponse>.FailResponse("User record not found", 404);
            }
            catch (Exception ex)
            {
                return ApiResponse<UploadStatusResponse>.FailResponse(ex.Message, 500);
            }
        }

        private async Task<bool> CallSmsApi(string mobile, string message)
        {
            try { return true; } catch { return false; }
        }
    }
}
