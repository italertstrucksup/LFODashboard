using DataAccessInterface;
using Microsoft.Data.SqlClient;
using MasterAPIServiceDAL.Interface;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection.Metadata.Ecma335;


namespace MasterAPIServiceDAL.Implmentation
{
    public class MasterDAL : IMasterDAL
    {
        private readonly string _connStr;
        private readonly IDataAccess _dataAccess;

        public MasterDAL(IConfiguration configuration, IDataAccess dataAccess)
        {
            _connStr = configuration.GetConnectionString("DefaultConnection");
            _dataAccess = dataAccess;
        }

        public async Task<DataTable> GetOperatorMaster(int Id)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Id", Id)
            };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "usp_OperatorMaster",
                parameters
            );

            return result;
        }

        public async Task<DataTable> GetDocumentMaster(int Id)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Id", Id)
            };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "usp_getDocumentMaster",
                parameters
            );

            return result;
        }

        public async Task<DataTable> GetVehicleDetailsMaster(string Action, int ? BodyId ,int ? TyreId )
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", Action),
                new SqlParameter("@BodyId", BodyId.HasValue ? (object)BodyId.Value : DBNull.Value),
                new SqlParameter("@TyreId", TyreId.HasValue ? (object)TyreId.Value : DBNull.Value)
            };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "usp_GetVehicleDetailsMaster",
                parameters
            );

            return result;
        }

        public async Task<DataTable> GetLocationByPincode(string Pincode)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Pincode", Pincode)
            };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "usp_GetLocationByPincode",
                parameters
                );
            return result;
        }

        public async Task<DataTable> GetCityMaster(string city)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@City", city ?? (object)DBNull.Value)
            };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "usp_GetCityList",
                parameters
                );
            return result;
        }

        public async Task<DataTable> GetKYCMaster(int Id)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Id", Id)
            };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "usp_getKYCMaster",
                parameters
            );

            return result;
        }


    }
}
