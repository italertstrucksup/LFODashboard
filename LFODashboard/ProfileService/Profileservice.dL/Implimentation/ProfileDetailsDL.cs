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

        // ❗ Keep as is (you may later convert to DTO)
       

        public async Task<string> UpdateFleetOperator(UpdateFleetOperatorRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) { Value = request.UserId },
                new SqlParameter("@CompanyName", SqlDbType.VarChar, 200) { Value = (object?)request.CompanyName ?? DBNull.Value },
                new SqlParameter("@CompanyAddress", SqlDbType.VarChar, 500) { Value = (object?)request.CompanyAddress ?? DBNull.Value },
                new SqlParameter("@PinCode", SqlDbType.VarChar, 10) { Value = (object?)request.Pincode ?? DBNull.Value },
                new SqlParameter("@City", SqlDbType.VarChar, 100) { Value = (object?)request.City ?? DBNull.Value },
                new SqlParameter("@SubCity", SqlDbType.VarChar, 100) { Value = (object?)request.SubCity ?? DBNull.Value },
                new SqlParameter("@State", SqlDbType.VarChar, 100) { Value = (object?)request.State ?? DBNull.Value }
            };

            var dt = await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "USP_UpdateFleetOperator", parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                var message = dt.Rows[0]["Message"]?.ToString();
                var isSuccess = Convert.ToInt32(dt.Rows[0]["IsSuccess"]) == 1;

                if (!isSuccess)
                    throw new Exception(message);

                return message ?? "Fleet operator updated successfully";
            }

            throw new Exception("No response from DB");
        }

        public async Task<string> InsertFleetOperatorbyType(UpdateFleetOperatorRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) { Value = request.UserId },
                new SqlParameter("@OwnerName", SqlDbType.VarChar, 255) { Value = (object?)request.OwnerName ?? DBNull.Value },
                new SqlParameter("@OperatorType", SqlDbType.Int) { Value = (object?)request.OperatorType ?? DBNull.Value },
                new SqlParameter("@ProfileImage", SqlDbType.VarChar, 255) { Value = (object?)request.ProfileImage ?? DBNull.Value }

            };

            var dt = await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "USP_InsertFleetOperatorbyType", parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                var message = dt.Rows[0]["Message"]?.ToString();
                var isSuccess = Convert.ToInt32(dt.Rows[0]["IsSuccess"]) == 1;

                if (!isSuccess)
                    throw new Exception(message);

                return message ?? "Inserted successfully";
            }

            throw new Exception("No response from DB");
        }

        public async Task<string> InsertPreferredLane(PreferredLaneRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) { Value = request.UserId },
                new SqlParameter("@FromLocation", SqlDbType.VarChar, 100) { Value = request.FromLocation },
                new SqlParameter("@ToLocation", SqlDbType.VarChar, 100) { Value = request.ToLocation }
            };

            var dt = await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "USP_InsertPreferredLane", parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                var message = dt.Rows[0]["Message"]?.ToString();
                var isSuccess = Convert.ToInt32(dt.Rows[0]["IsSuccess"]) == 1;

                if (!isSuccess)
                    throw new Exception(message);

                return message ?? "Inserted successfully";
            }

            throw new Exception("No response from DB");
        }

        public async Task<string> InsertFleetOperatorDocument(UpdateDocumentRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) { Value = request.UserId },
                new SqlParameter("@DocumentType", SqlDbType.VarChar, 50) { Value = request.DocumentType },
                new SqlParameter("@DocumentUrl", SqlDbType.VarChar, 500) { Value = request.DocumentUrl }
            };

            var dt = await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "USP_InsertFleetOperatorDocument", parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                var message = dt.Rows[0]["Message"]?.ToString();
                var isSuccess = Convert.ToInt32(dt.Rows[0]["IsSuccess"]) == 1;

                if (!isSuccess)
                    throw new Exception(message);

                return message ?? "Document inserted successfully";
            }

            throw new Exception("No response from DB");
        }

        public async Task<string> InsertTruckDetails(TruckDetailsRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) { Value = request.UserId },
                new SqlParameter("@VehicleNo", SqlDbType.VarChar, 20) { Value = request.VehicleNo },
                new SqlParameter("@OwnershipType", SqlDbType.VarChar, 20) { Value = (object?)request.OwnershipType ?? DBNull.Value },
                new SqlParameter("@BodyTypeId", SqlDbType.Int) { Value = request.BodyTypeId },
                new SqlParameter("@TyreId", SqlDbType.Int) { Value = request.TyreId },
                new SqlParameter("@CapacityId", SqlDbType.Int) { Value = request.CapacityId },
                new SqlParameter("@SizeId", SqlDbType.Int) { Value = request.SizeId }
            };

            var dt = await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "USP_MasterVehicleDetails", parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                var message = dt.Rows[0]["Message"]?.ToString();
                var isSuccess = Convert.ToInt32(dt.Rows[0]["IsSuccess"]) == 1;

                if (!isSuccess)
                    throw new Exception(message);

                return message ?? "Truck inserted successfully";
            }

            throw new Exception("No response from DB");
        }

        public async Task<string> InsertFleetOperatorKYC(KYCRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) { Value = request.UserId },
                new SqlParameter("@KYCType", SqlDbType.VarChar, 50) { Value = request.KYCType },
                new SqlParameter("@KYCNumber", SqlDbType.VarChar, 50) { Value = request.KYCNumber },
                new SqlParameter("@KYCDocFront", SqlDbType.VarChar, 50) { Value = request.KYCDocFront },
                new SqlParameter("@KYCDocBack", SqlDbType.VarChar, 50) { Value = request.KYCDocBack }
            };

            var dt = await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "USP_InsertFleetOperatorKYC", parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                var message = dt.Rows[0]["Message"]?.ToString();
                var isSuccess = Convert.ToInt32(dt.Rows[0]["IsSuccess"]) == 1;

                if (!isSuccess)
                    throw new Exception(message);

                return message ?? "KYC inserted successfully";
            }

            throw new Exception("No response from DB");
        }
    }
}