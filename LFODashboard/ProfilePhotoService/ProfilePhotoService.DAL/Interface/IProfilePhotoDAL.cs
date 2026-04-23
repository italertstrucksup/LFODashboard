using System.Data;

namespace ProfilePhotoService.DAL.Interface
{
    public interface IProfilePhotoDAL
    {
        Task<DataTable> SaveUploadLinkAsync(Guid loginId, string mobileNo, string token);
        Task<DataTable> ValidateUploadTokenAsync(string token);
        Task<DataTable> UpdateProfilePhotoAsync(Guid loginId, string photoPath, string photoType);
        Task<DataTable> GetUploadStatusAsync(Guid loginId, string photoType);
        Task<DataTable> ExpireTokenAsync(string token);
    }
}
