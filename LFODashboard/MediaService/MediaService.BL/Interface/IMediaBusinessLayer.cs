using MediaService.Model.Model;
using Microsoft.AspNetCore.Http;

namespace MediaService.BL.IBussinessLayer
{
    public interface IMediaBusinessLayer
    {
        Task<MediaResponse> UploadDocumentAsync(IFormFile file, string folderName);
        Task<MediaResponse> GetDocumentAsync(string documentKey);
        MediaResponse GetPreSignedUrl(string documentKey);
        //Task<bool> DeleteDocumentAsync(string documentKey);
    }
}
