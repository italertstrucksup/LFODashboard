using Common.Core;
using ProfilePhotoService.Model.Models;

namespace ProfilePhotoService.BL.Interface
{
    public interface IProfilePhotoBL
    {
        Task<ApiResponse<string>> SendUploadLinkAsync(UploadLinkRequest request);
        Task<ApiResponse<string>> ResendUploadLinkAsync(UploadLinkRequest request);
        Task<ApiResponse<object>> UploadPhotoAsync(PhotoUploadRequest request);
        Task<ApiResponse<bool>> ValidateTokenAsync(string token);
        Task<ApiResponse<UploadStatusResponse>> GetUploadStatusAsync(Guid loginId);
    }
}
