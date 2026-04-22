using DataAccessInterface;
using Microsoft.Data.SqlClient;
using ProfileService_LFO.Model.Model;

using Microsoft.Extensions.Configuration;
using ProfileService_LFO.DAL.Interface;
using System.Data;

namespace ProfileService_LFO.DAL.Implimentation
{
    public class ProfileDetailsDL : IprofileDetailsDL
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
            var result = await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "USP_GetFleetOperatorByUserId", parameters);

            return result;
        }

        public async Task<bool> UpdateFleetOperator(UpdateFleetOperatorRequest request)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@Id", SqlDbType.BigInt) { Value = request.UserId },

        new SqlParameter("@CompanyName", SqlDbType.VarChar, 200) { Value = (object?)request.CompanyName ?? DBNull.Value },
        new SqlParameter("@CompanyAddress", SqlDbType.VarChar, 500) { Value = (object?)request.CompanyAddress ?? DBNull.Value },
        new SqlParameter("@Pincode", SqlDbType.VarChar, 10) { Value = (object?)request.Pincode ?? DBNull.Value },
        new SqlParameter("@City", SqlDbType.VarChar, 100) { Value = (object?)request.City ?? DBNull.Value },
        new SqlParameter("@SubCity", SqlDbType.VarChar, 100) { Value = (object?)request.SubCity ?? DBNull.Value },
        new SqlParameter("@State", SqlDbType.VarChar, 100) { Value = (object?)request.State ?? DBNull.Value },

        new SqlParameter("@UpdatedBy", SqlDbType.VarChar, 50) { Value = (object?)request.UpdatedBy ?? DBNull.Value }
    };

            var result = await _dataAccess.ExecuteNonQueryAsync(_connStr, "USP_UpdateFleetOperator", parameters);

            return result > 0;
        }

        public async Task<bool> InsertFleetOperatorbyType(UpdateFleetOperatorRequest request)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@Id", SqlDbType.BigInt) { Value = request.UserId },

        new SqlParameter("@ownerName", SqlDbType.VarChar, 200) { Value = (object?)request.OwnerName ?? DBNull.Value },
        new SqlParameter("@OpretarType", SqlDbType.VarChar, 500) { Value = (object?)request.OpretarType ?? DBNull.Value },

        new SqlParameter("@UpdatedBy", SqlDbType.VarChar, 50) { Value = (object?)request.UpdatedBy ?? DBNull.Value }
    };

            var result = await _dataAccess.ExecuteNonQueryAsync(_connStr, "USP_InsertFleetOperatorbyType", parameters);
            return result > 0;
        }

        public async Task<bool> InsertFleetOperatorDocument(UpdateDocumentRequest request)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@OperatorId", SqlDbType.BigInt) { Value = request.OperatorId },

        new SqlParameter("@COI_File", SqlDbType.VarChar, 500) { Value = (object?)request.DocumentType ?? DBNull.Value },
        new SqlParameter("@PAN_File", SqlDbType.VarChar, 500) { Value = (object?)request.DocumentUrl ?? DBNull.Value },
        
    };

            var result = await _dataAccess.ExecuteNonQueryAsync(
                _connStr,
                "USP_InsertFleetOperatorDocument",
                parameters
            );

            return result > 0;
        }

        #region Insert
        public async Task<bool> InsertTruckDetails(TruckDetailsRequest request)
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
                "USP_InsertFleetDetails",
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
        public async Task<bool> InsertPreferredLane(PreferredLaneRequest request)
        {
            var parameters = new List<SqlParameter>
        {
            new SqlParameter("@LoginId", SqlDbType.BigInt) { Value = request.OperatorId },
            new SqlParameter("@FromLocation", SqlDbType.VarChar, 100) { Value = request.FromLocation },
            new SqlParameter("@ToLocation", SqlDbType.VarChar, 100) { Value = request.ToLocation },
          
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

        #region Insert
        public async Task<bool> InsertFleetOperatorKYC(KYCRequest request)
        {
            var parameters = new List<SqlParameter>
        {
            new SqlParameter("@OperatorId", SqlDbType.BigInt) { Value = request.ProfileId },
            new SqlParameter("@KYCType", SqlDbType.VarChar, 50) { Value = request.KYCType },
            new SqlParameter("@KYCNumber", SqlDbType.VarChar, 50) { Value = request.KYCNumber },
            new SqlParameter("@KYCDocFront", SqlDbType.VarChar, 50) { Value = request.KYCDocFront },
            new SqlParameter("@KYCDocBack", SqlDbType.VarChar, 50) { Value = request.KYCDocBack }
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
