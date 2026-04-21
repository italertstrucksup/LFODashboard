using DataAccessInterface;
using Microsoft.Data.SqlClient;
using ProfileService_LFO.Model.Models;

using Microsoft.Extensions.Configuration;
using ProfileService_LFO.DAL.Interface;
using System.Data;

namespace ProfileService_LFO.DAL.Implimentation
{
    public class ProfileDetailsDL
    {
        private readonly string _connStr;
        private readonly IDataAccess _dataAccess;

        public ProfileDetailsDL(IConfiguration configuration, IDataAccess dataAccess)
        {
            _connStr = configuration.GetConnectionString("DefaultConnection");
            _dataAccess = dataAccess;
        }

        public async Task<DataTable> GetProfileDetailsbyID(int userId)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@Action", "GET_PROFILE_DETAILS"),
        new SqlParameter("@UserId", userId)
    };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "LFO_SP_ProfileDetails",
                parameters
            );

            return result;
        }

        public async Task<bool> UpsertProfileAsync(UpsertProfileRequest request)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@LoginId", SqlDbType.BigInt) { Value = request.LoginId },
        new SqlParameter("@ProfileName", SqlDbType.VarChar, 200) { Value = (object?)request.ProfileName ?? DBNull.Value },
        new SqlParameter("@MobileNo", SqlDbType.VarChar, 15) { Value = (object?)request.MobileNo ?? DBNull.Value },
        new SqlParameter("@CompanyType", SqlDbType.VarChar, 100) { Value = (object?)request.CompanyType ?? DBNull.Value },
        new SqlParameter("@CompanyName", SqlDbType.VarChar, 200) { Value = (object?)request.CompanyName ?? DBNull.Value },
        new SqlParameter("@Pincode", SqlDbType.VarChar, 10) { Value = (object?)request.Pincode ?? DBNull.Value },
        new SqlParameter("@CompanyAddress", SqlDbType.VarChar, 500) { Value = (object?)request.CompanyAddress ?? DBNull.Value },
        new SqlParameter("@City", SqlDbType.VarChar, 100) { Value = (object?)request.City ?? DBNull.Value },
        new SqlParameter("@SubCity", SqlDbType.VarChar, 100) { Value = (object?)request.SubCity ?? DBNull.Value },
        new SqlParameter("@State", SqlDbType.VarChar, 100) { Value = (object?)request.State ?? DBNull.Value },
        new SqlParameter("@ProfilePhoto", SqlDbType.VarChar, 200) { Value = (object?)request.ProfilePhoto ?? DBNull.Value },
        new SqlParameter("@IsProfilePhotoUploaded", SqlDbType.Bit) { Value = request.IsProfilePhotoUploaded },
        new SqlParameter("@ProfilePhotoLink", SqlDbType.VarChar, 200) { Value = (object?)request.ProfilePhotoLink ?? DBNull.Value },
        new SqlParameter("@ProfilePhotoLinkExpiry", SqlDbType.DateTime) { Value = (object?)request.ProfilePhotoLinkExpiry ?? DBNull.Value },
        new SqlParameter("@IsKYCDone", SqlDbType.Bit) { Value = request.IsKYCDone },
        new SqlParameter("@KYCType", SqlDbType.VarChar, 50) { Value = (object?)request.KYCType ?? DBNull.Value },
        new SqlParameter("@CreatedBy", SqlDbType.VarChar, 50) { Value = (object?)request.CreatedBy ?? DBNull.Value }
    };

            var result = await _dataAccess.ExecuteNonQueryAsync(
                _connStr,
                "SP_UpsertProfileDetails",
                parameters
            );

            return result > 0;
        }

        public async Task<bool> UpsertDocumentsAsync(UpsertDocumentRequest request)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@ProfileId", SqlDbType.BigInt) { Value = request.ProfileId },

        new SqlParameter("@COI_File", SqlDbType.VarChar, 500) { Value = (object?)request.COI_File ?? DBNull.Value },
        new SqlParameter("@PAN_File", SqlDbType.VarChar, 500) { Value = (object?)request.PAN_File ?? DBNull.Value },
        new SqlParameter("@MAA_File", SqlDbType.VarChar, 500) { Value = (object?)request.MAA_File ?? DBNull.Value },
        new SqlParameter("@GST_File", SqlDbType.VarChar, 500) { Value = (object?)request.GST_File ?? DBNull.Value },
        new SqlParameter("@RC_File", SqlDbType.VarChar, 500) { Value = (object?)request.RC_File ?? DBNull.Value },
        new SqlParameter("@Partnership_File", SqlDbType.VarChar, 500) { Value = (object?)request.Partnership_File ?? DBNull.Value },

        new SqlParameter("@VerifiedBy", SqlDbType.VarChar, 50) { Value = (object?)request.VerifiedBy ?? DBNull.Value }
    };

            var result = await _dataAccess.ExecuteNonQueryAsync(
                _connStr,
                "SP_UpsertDocuments",
                parameters
            );

            return result > 0;
        }

        #region Insert
        public async Task<bool> InsertTruckAsync(TruckDetailsRequest request)
        {
            var parameters = new List<SqlParameter>
        {
            new SqlParameter("@ProfileId", SqlDbType.BigInt) { Value = request.ProfileId },
            new SqlParameter("@TruckNumber", SqlDbType.VarChar, 20) { Value = request.TruckNumber },
            new SqlParameter("@OwnershipType", SqlDbType.VarChar, 20) { Value = (object?)request.OwnershipType ?? DBNull.Value },
            new SqlParameter("@BodyType", SqlDbType.VarChar, 100) { Value = request.BodyType },
            new SqlParameter("@TyreCount", SqlDbType.Int) { Value = request.TyreCount },
            new SqlParameter("@Capacity", SqlDbType.Decimal) { Value = request.Capacity },
            new SqlParameter("@VehicleSize", SqlDbType.VarChar, 50) { Value = request.VehicleSize }
        };

            var result = await _dataAccess.ExecuteNonQueryAsync(
                _connStr,
                "SP_InsertTruckDetails",
                parameters
            );

            return result > 0;
        }
        #endregion

        #region Get
        public async Task<DataTable> GetTrucksByProfileId(long profileId)
        {
            var parameters = new List<SqlParameter>
        {
            new SqlParameter("@ProfileId", SqlDbType.BigInt) { Value = profileId }
        };

            return await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "SP_GetTrucksByProfileId",
                parameters
            );
        }
        #endregion

        #region Insert
        public async Task<bool> InsertLaneAsync(PreferredLaneRequest request)
        {
            var parameters = new List<SqlParameter>
        {
            new SqlParameter("@LoginId", SqlDbType.BigInt) { Value = request.LoginId },
            new SqlParameter("@FromCity", SqlDbType.VarChar, 100) { Value = request.FromCity },
            new SqlParameter("@ToCity", SqlDbType.VarChar, 100) { Value = request.ToCity },
            new SqlParameter("@FromState", SqlDbType.VarChar, 100) { Value = request.FromState },
            new SqlParameter("@ToState", SqlDbType.VarChar, 100) { Value = request.ToState }
        };

            var result = await _dataAccess.ExecuteNonQueryAsync(
                _connStr,
                "SP_InsertPreferredLane",
                parameters
            );

            return result > 0;
        }
        #endregion
        #region Get
        public async Task<DataTable> GetLanesAsync(long loginId)
        {
            var parameters = new List<SqlParameter>
        {
            new SqlParameter("@LoginId", SqlDbType.BigInt) { Value = loginId }
        };

            return await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "SP_GetPreferredLanes",
                parameters
            );
        }
        #endregion

        #region Upsert
        public async Task<bool> UpsertKYCAsync(KYCRequest request)
        {
            var parameters = new List<SqlParameter>
        {
            new SqlParameter("@ProfileId", SqlDbType.BigInt) { Value = request.ProfileId },
            new SqlParameter("@KYCType", SqlDbType.VarChar, 50) { Value = request.KYCType }
        };

            var result = await _dataAccess.ExecuteNonQueryAsync(
                _connStr,
                "SP_UpsertKYC",
                parameters
            );

            return result > 0;
        }
        #endregion

        #region Get
        public async Task<DataTable> GetKYCAsync(long profileId)
        {
            var parameters = new List<SqlParameter>
        {
            new SqlParameter("@ProfileId", SqlDbType.BigInt) { Value = profileId }
        };

            return await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "SP_GetKYCByProfileId",
                parameters
            );
        }
        #endregion

        #region
        public async Task<bool> UpsertKYCDocumentsAsync(KYCDocumentRequest request)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@KYCId", SqlDbType.BigInt) { Value = request.KYCId },

        new SqlParameter("@ProfilePhoto", SqlDbType.VarChar, 500) { Value = (object?)request.ProfilePhoto ?? DBNull.Value },

        new SqlParameter("@AadhaarNumber", SqlDbType.VarChar, 20) { Value = (object?)request.AadhaarNumber ?? DBNull.Value },
        new SqlParameter("@AadhaarFront", SqlDbType.VarChar, 500) { Value = (object?)request.AadhaarFront ?? DBNull.Value },
        new SqlParameter("@AadhaarBack", SqlDbType.VarChar, 500) { Value = (object?)request.AadhaarBack ?? DBNull.Value },

        new SqlParameter("@PANNumber", SqlDbType.VarChar, 20) { Value = (object?)request.PANNumber ?? DBNull.Value },
        new SqlParameter("@PANFile", SqlDbType.VarChar, 500) { Value = (object?)request.PANFile ?? DBNull.Value },

        new SqlParameter("@SelfieKey", SqlDbType.VarChar, 500) { Value = (object?)request.SelfieKey ?? DBNull.Value },
        new SqlParameter("@IsSelfieUploaded", SqlDbType.Bit) { Value = request.IsSelfieUploaded }
    };

            var result = await _dataAccess.ExecuteNonQueryAsync(
                _connStr,
                "SP_UpsertKYCDocuments",
                parameters
            );

            return result > 0;
        }

#endregion

    }
}
