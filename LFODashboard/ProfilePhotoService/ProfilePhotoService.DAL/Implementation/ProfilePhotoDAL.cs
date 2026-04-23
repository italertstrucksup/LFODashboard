using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProfilePhotoService.DAL.Interface;
using System.Data;
using DataAccessInterface;

namespace ProfilePhotoService.DAL.Implemetation
{
    public class ProfilePhotoDAL : IProfilePhotoDAL
    {
        private readonly string _connStr;
        private readonly IDataAccess _dataAccess;

        public ProfilePhotoDAL(IConfiguration configuration, IDataAccess dataAccess)
        {
            _connStr = configuration.GetConnectionString("DefaultConnection") ??
                       configuration.GetSection("ConnectionStrings")["DefaultConnection"] ?? "";
            _dataAccess = dataAccess;

            if (string.IsNullOrEmpty(_connStr))
            {
                throw new InvalidOperationException("Database Connection String 'DefaultConnection' is not initialized.");
            }
        }

        public async Task<DataTable> SaveUploadLinkAsync(Guid loginId, string mobileNo, string token)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", loginId), // Matching SP parameter name @UserId
                new SqlParameter("@Token", token)
            };

            return await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "usp_SaveProfilePhotoLink", parameters);
        }

        public async Task<DataTable> ValidateUploadTokenAsync(string token)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Token", token)
            };

            return await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "usp_ValidateProfilePhotoToken", parameters);
        }

        public async Task<DataTable> UpdateProfilePhotoAsync(Guid loginId, string photoPath, string photoType)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", loginId),
                new SqlParameter("@PhotoPath", photoPath),
                new SqlParameter("@PhotoType", photoType)
            };

            return await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "usp_UpdateProfilePhoto", parameters);
        }

        public async Task<DataTable> GetUploadStatusAsync(Guid loginId, string photoType)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", loginId),
                new SqlParameter("@PhotoType", photoType)
            };

            return await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "usp_GetProfilePhotoStatus", parameters);
        }

        public async Task<DataTable> ExpireTokenAsync(string token)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Token", token)
            };
            return await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "usp_ValidateProfilePhotoToken", parameters);
        }
    }
}
