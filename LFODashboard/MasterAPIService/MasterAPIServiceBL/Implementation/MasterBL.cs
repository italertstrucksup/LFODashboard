using Common.Core;
using DataAccessInterface;
using HttpClientLib;
using MasterAPIServiceBL.Interface;
using MasterAPIServiceDAL.Interface;
using MasterAPIServiceModel.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MasterAPIServiceModel.Models;
using System.Data;
using System.Text;
using System.Text.Encodings.Web;


namespace MasterAPIServiceBL.Implementation
{
    public class MasterBL : IMasterBL
    {
        private readonly IConfiguration _config;
        private readonly IMasterDAL _masterDAL;
        private readonly IDataAccess _dataAccess;
        private readonly IHttpService _httpService;


        public MasterBL(IConfiguration config, IDataAccess dataAccess, IMasterDAL masterDAL, IHttpService httpService)
        {
            _config = config;
            _dataAccess = dataAccess;
            _masterDAL = masterDAL;
            _httpService = httpService;
        }

        #region GetOperatorMaster

        public async Task<ApiResponse<OperatorMasterResponse>> GetOperatorMaster(int id)
        {
            try
            {
                var result = await _masterDAL.GetOperatorMaster(id);

                if (result.Rows.Count == 0)
                {
                    return ApiResponse<OperatorMasterResponse>.FailResponse(
                        "Operator not found",
                        statusCode: 404
                    );
                }

                var row = result.Rows[0];

                var fleetOperatorType = new OperatorMasterResponse
                {
                    Id = Convert.ToInt32(row["id"]),
                    Code = row["code"]?.ToString(),
                    Name = row["name"]?.ToString()
                };

                return ApiResponse<OperatorMasterResponse>.SuccessResponse(
                    fleetOperatorType,
                    message: "Operator fetched successfully"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<OperatorMasterResponse>.FailResponse(
                    ex.Message,
                    statusCode: 500
                );
            }
        }

        #endregion


        #region GetDocumentMaster

        public async Task<ApiResponse<DocumentMasterResponse>> GetDocumentMaster(int id)
        {
            try
            {
                var result = await _masterDAL.GetDocumentMaster(id);

                if (result.Rows.Count == 0)
                {
                    return ApiResponse<DocumentMasterResponse>.FailResponse(
                        "Operator not found",
                        statusCode: 404
                    );
                }

                var row = result.Rows[0];

                var documentData = new DocumentMasterResponse
                {
                    Id = Convert.ToInt32(row["id"]),
                    Code = row["code"]?.ToString(),
                    Name = row["name"]?.ToString()
                };

                return ApiResponse<DocumentMasterResponse>.SuccessResponse(
                    documentData,
                    message: "Operator fetched successfully"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<DocumentMasterResponse>.FailResponse(
                    ex.Message,
                    statusCode: 500
                );
            }
        }

        #endregion

        #region GetVehicleDetailsMaster

        public async Task<ApiResponse<List<VehicleDetailsMasterResponse>>> GetVehicleDetailsMaster(string actionType, int? bodyId, int? tyreId)
        {
            try
            {
                var result = await _masterDAL.GetVehicleDetailsMaster(actionType, bodyId, tyreId);

                if (result.Rows.Count == 0)
                {
                    return ApiResponse<List<VehicleDetailsMasterResponse>>.FailResponse(
                        "Vehicle details not found",
                        statusCode: 404
                    );
                }

                var vehicleList = new List<VehicleDetailsMasterResponse>();

                foreach (DataRow row in result.Rows)
                {

                    if (actionType == "body")
                    {
                        vehicleList.Add(new VehicleDetailsMasterResponse
                        {
                            Id = Convert.ToInt32(row["body_type_id"]),
                            Name = row["type_name"]?.ToString()
                        });
                    }
                    else if (actionType == "tyretype")
                    {
                        vehicleList.Add(new VehicleDetailsMasterResponse
                        {
                            Id = Convert.ToInt32(row["tyre_id"]),
                            Name = row["tyre_type"]?.ToString()
                        });
                    }
                    else if (actionType == "capacity")
                    {
                        vehicleList.Add(new VehicleDetailsMasterResponse
                        {
                            Id = Convert.ToInt32(row["capacity_id"]),
                            Name = row["capacity_type"]?.ToString()
                        });
                    }
                    else if (actionType == "vehiclesize")
                    {
                        vehicleList.Add(new VehicleDetailsMasterResponse
                        {
                            Id = Convert.ToInt32(row["size_id"]),
                            Name = row["size_type"]?.ToString()
                        });
                    }
                }

                return ApiResponse<List<VehicleDetailsMasterResponse>>.SuccessResponse(
                    vehicleList,
                    message: "Vehicle details fetched successfully"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<List<VehicleDetailsMasterResponse>>.FailResponse(
                    ex.Message,
                    statusCode: 500
                );
            }
        }

        #endregion


        #region GetLocationByPincode
        public async Task<ApiResponse<List<GetLocationByPincodeResponse>>> GetLocationByPincode(string pincode)
        {
            try
            {
                var result = await _masterDAL.GetLocationByPincode(pincode);
                if (result.Rows.Count == 0)
                {
                    return ApiResponse<List<GetLocationByPincodeResponse>>.FailResponse(
                        "Location not found for the given pincode",
                        statusCode: 404
                    );
                }
               

                var locationInfo = new List<GetLocationByPincodeResponse>();
                foreach (DataRow row in result.Rows)
                {
                    locationInfo.Add(new GetLocationByPincodeResponse

                    {
                        CircleName = row["CircleName"] == DBNull.Value ? null : row["CircleName"]?.ToString(),
                        RegionName = row["RegionName"] == DBNull.Value ? null : row["RegionName"]?.ToString(),
                        DivisionName = row["DivisionName"] == DBNull.Value ? null : row["DivisionName"]?.ToString(),
                        SubCity = row["OfficeName"] == DBNull.Value ? null : row["OfficeName"]?.ToString(),
                        PinCode = row["PinCode"] == DBNull.Value ? null : row["PinCode"]?.ToString(),
                        OfficeType = row["OfficeType"] == DBNull.Value ? null : row["OfficeType"]?.ToString(),
                        Delivery = row["Delivery"] == DBNull.Value ? null : row["Delivery"]?.ToString(),
                        City = row["District"] == DBNull.Value ? null : row["District"]?.ToString(),
                        State = row["StateName"] == DBNull.Value ? null : row["StateName"]?.ToString(),
                        Latitude = row["Latitude"] == DBNull.Value ? 0 : Convert.ToDecimal(row["Latitude"]),
                        Longitude = row["Longitude"] == DBNull.Value ? 0 : Convert.ToDecimal(row["Longitude"]),
                        ActiveFlag = row["ActiveFlag"] == DBNull.Value ? false : Convert.ToBoolean(row["ActiveFlag"]),
                        CreatedAt = row["CreatedAt"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreatedAt"])
                    });
                }
                return ApiResponse<List<GetLocationByPincodeResponse>>.SuccessResponse(
                    locationInfo,
                    message: "Location information fetched successfully"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetLocationByPincodeResponse>>.FailResponse(
                    ex.Message,
                    statusCode: 500
                );
            }
        }

        #endregion

        #region GetCityMaster

        public async Task<ApiResponse<List<CityMasterResponse>>> GetCityMaster(string city)
        {
            try
            {
                var result = await _masterDAL.GetCityMaster(city);

                if (result.Rows.Count == 0)
                {
                    return ApiResponse<List<CityMasterResponse>>.FailResponse(
                        "No cities found",
                        statusCode: 404
                    );
                }

                var cityList = new List<CityMasterResponse>();

                foreach (DataRow row in result.Rows)
                {
                    cityList.Add(new CityMasterResponse
                    {
                        SrNo = Convert.ToInt32(row["Srno"]),
                        City = row["city"]?.ToString(),
                        State = row["state"]?.ToString(),
                        Latitude = Convert.ToDecimal(row["latitude"]),
                        Longitude = Convert.ToDecimal(row["Longitude"]),
                        Pincode1 = Convert.ToInt32(row["pincode1"]),
                        Pincode2 = Convert.ToInt32(row["Pincode2"])
                    });
                }

                return ApiResponse<List<CityMasterResponse>>.SuccessResponse(
                    cityList,
                    message: "Cities fetched successfully"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CityMasterResponse>>.FailResponse(
                    ex.Message,
                    statusCode: 500
                );
            }
        }

        #endregion

        
        #region GetKYCMaster

        public async Task<ApiResponse<KYCMasterResponse>> GetKYCMaster(int id)
        {
            try
            {
                var result = await _masterDAL.GetKYCMaster(id);

                if (result.Rows.Count == 0)
                {
                    return ApiResponse<KYCMasterResponse>.FailResponse(
                        "KYC not found",
                        statusCode: 404
                    );
                }

                var row = result.Rows[0];

                var KYCType = new KYCMasterResponse
                {
                    Id = Convert.ToInt32(row["id"]),
                    Code = row["code"]?.ToString(),
                    Name = row["name"]?.ToString()
                };

                return ApiResponse<KYCMasterResponse>.SuccessResponse(
                    KYCType,
                    message: "KYC fetched successfully"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<KYCMasterResponse>.FailResponse(
                    ex.Message,
                    statusCode: 500
                );
            }
        }

        #endregion

    }
}
