using Common.Core;
using Microsoft.AspNetCore.Http;

namespace MediaService.BL.IBussinessLayer
{
    public interface IMediaBusinessLayer
    {
        Task<ApiResponse<object>> UploadDocumentAsync(IFormFile file, string folderName);
        Task<ApiResponse<object>> GetDocumentAsync(string documentKey);
        ApiResponse<object> GetPreSignedUrl(string documentKey);
    }
}
