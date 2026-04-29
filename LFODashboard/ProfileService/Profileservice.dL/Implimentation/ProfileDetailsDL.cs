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

        #region Get Profile By Id
        public async Task<DataTable> GetProfileDetailsbyID(Guid userId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", SqlDbType.UniqueIdentifier)
                {
                    Value = userId
                }
            };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "USP_GetFleetOperatorByUserId",
                parameters
            );

            return result;
        }
        #endregion

        #region Get Complete KYC Data
        public async Task<DataSet> GetCompleteKYCDataAsync(Guid userId)
        {
            var ds = new DataSet();

            using (var conn = new SqlConnection(_connStr))
            {
                using (var cmd = new SqlCommand("USP_GetCompleteKYCData", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(
                        new SqlParameter("@UserId", SqlDbType.UniqueIdentifier)
                        {
                            Value = userId
                        });

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(ds);
                    }
                }
            }

            return ds;
        }
        #endregion

        #region Update Fleet Operator
        public async Task<string> UpdateFleetOperator(UpdateProfileRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", SqlDbType.UniqueIdentifier)
                {
                    Value = request.UserId
                },

                new SqlParameter("@CompanyName", SqlDbType.VarChar, 200)
                {
                    Value = (object?)request.CompanyName ?? DBNull.Value
                },

                new SqlParameter("@CompanyAddress", SqlDbType.VarChar, 500)
                {
                    Value = (object?)request.CompanyAddress ?? DBNull.Value
                },

                new SqlParameter("@PinCode", SqlDbType.VarChar, 10)
                {
                    Value = (object?)request.Pincode ?? DBNull.Value
                },

                new SqlParameter("@City", SqlDbType.VarChar, 100)
                {
                    Value = (object?)request.City ?? DBNull.Value
                },

                new SqlParameter("@SubCity", SqlDbType.VarChar, 100)
                {
                    Value = (object?)request.SubCity ?? DBNull.Value
                },

                new SqlParameter("@State", SqlDbType.VarChar, 100)
                {
                    Value = (object?)request.State ?? DBNull.Value
                }
            };

            await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "USP_UpdateFleetOperator",
                parameters
            );

            return "Fleet operator updated successfully";
        }
        #endregion

        #region Insert Fleet Operator By Type
        public async Task<string> InsertFleetOperatorbyType(UpdateFleetOperatorRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", SqlDbType.UniqueIdentifier)
                {
                    Value = request.UserId
                },

                new SqlParameter("@OwnerName", SqlDbType.VarChar, 255)
                {
                    Value = (object?)request.OwnerName ?? DBNull.Value
                },

                new SqlParameter("@OperatorType", SqlDbType.Int)
                {
                    Value = (object?)request.OperatorType ?? DBNull.Value
                },
                 new SqlParameter("@ProfileImage", SqlDbType.VarChar, 255)
                {
                    Value = (object?)request.ProfileImage ?? DBNull.Value
                }
            };

            await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "USP_InsertFleetOperatorbyType",
                parameters
            );

            return "Operator type inserted successfully";
        }
        #endregion

        #region Insert Preferred Lane
        public async Task<string> InsertPreferredLane(PreferredLaneRequest request)
        {
            if (request?.Lanes == null || !request.Lanes.Any())
                return "No lanes found";

            foreach (var lane in request.Lanes)
            {
                var parameters = new List<SqlParameter>
        {
            new SqlParameter("@UserId", SqlDbType.UniqueIdentifier)
            {
                Value = request.UserId
            },

            new SqlParameter("@FromLocation", SqlDbType.VarChar, 200)
            {
                Value = lane.FromLocation
            },

            new SqlParameter("@ToLocation", SqlDbType.VarChar, 200)
            {
                Value = lane.ToLocation
            },

            new SqlParameter("@from_state", SqlDbType.VarChar, 200)
            {
                Value = lane.FromState
            },

            new SqlParameter("@to_state", SqlDbType.VarChar, 200)
            {
                Value = lane.ToState
            }
        };

                await _dataAccess.ExecuteStoredProcedureAsync(
                    _connStr,
                    "USP_InsertPreferredLane",
                    parameters
                );
            }

            return "Preferred lanes inserted successfully";
        }
        #endregion

        #region Insert Fleet Operator Document
        public async Task<string> InsertFleetOperatorDocument(UpdateDocumentRequest request)
        {
            foreach (var item in request.documents)
            {
                var parameters = new List<SqlParameter>
        {
            new SqlParameter("@UserId", SqlDbType.UniqueIdentifier)
            {
                Value = request.UserId
            },

            new SqlParameter("@DocumentType", SqlDbType.VarChar, 100)
            {
                Value = item.DocumentType
            },

            new SqlParameter("@DocumentUrl", SqlDbType.VarChar, 500)
            {
                Value = item.DocumentUrl
            }
        };

                await _dataAccess.ExecuteStoredProcedureAsync(
                    _connStr,
                    "USP_InsertFleetOperatorDocument",
                    parameters
                );
            }

            return "Documents uploaded successfully";
        }
        #endregion

        #region Insert Truck Details
        public async Task<string> InsertTruckDetails(TruckDetailsRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", SqlDbType.UniqueIdentifier)
                {
                    Value = request.UserId
                },

                new SqlParameter("@VehicleNo", SqlDbType.VarChar, 20)
                {
                    Value = request.VehicleNo
                },

                new SqlParameter("@OwnershipType", SqlDbType.VarChar, 20)
                {
                    Value = (object?)request.OwnershipType ?? DBNull.Value
                },

                new SqlParameter("@BodyTypeId", SqlDbType.Int)
                {
                    Value = request.BodyTypeId
                },

                new SqlParameter("@TyreId", SqlDbType.Int)
                {
                    Value = request.TyreId
                },

                new SqlParameter("@CapacityId", SqlDbType.Int)
                {
                    Value = request.CapacityId
                },

                new SqlParameter("@SizeId", SqlDbType.Int)
                {
                    Value = request.SizeId
                }
            };

            await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "USP_MasterVehicleDetails",
                parameters
            );

            return "Truck inserted successfully";
        }
        #endregion

        #region Insert Fleet Operator KYC
        public async Task<string> InsertFleetOperatorKYC(KYCRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", SqlDbType.UniqueIdentifier)
                {
                    Value = request.UserId
                },

                new SqlParameter("@KYCType", SqlDbType.VarChar, 50)
                {
                    Value = request.KYCType
                },

                new SqlParameter("@KYCNumber", SqlDbType.VarChar, 50)
                {
                    Value = request.KYCNumber
                },

                new SqlParameter("@KYCDocFront", SqlDbType.VarChar, 50)
                {
                    Value = request.KYCDocFront
                },

                new SqlParameter("@KYCDocBack", SqlDbType.VarChar, 50)
                {
                    Value = request.KYCDocBack
                }
            };

            await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "USP_InsertFleetOperatorKYC",
                parameters
            );

            return "KYC inserted successfully";
        }
        #endregion
    }
}