using Common.Core;
using HttpClientLib;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using VendorService.DAL.Interface;
using VendorService.Models;

namespace VendorService.DAL.Repository
{
    public class AadhaarDAL : IAadhaarDAL
    {
        private readonly IHttpService _httpService;
        private readonly IConfiguration _config;
        private readonly CL_JWTtokenGeneration _cL_JWTtoken;


        public AadhaarDAL(IHttpService httpService,IConfiguration config, CL_JWTtokenGeneration cL_JWTtoken)
        {
            _httpService = httpService;
            _config = config;
            _cL_JWTtoken = cL_JWTtoken;
        }
        public async Task<AadharResponse> SendAadharOTPAsync(AadhaarRequest request)
        {
            string apiUrl = _config["AadhaarService:ApiBaseUrl"] +_config["AadhaarService:SendOtp"];

            string token = _cL_JWTtoken.GenerateToken();

            var headers = new Dictionary<string, string>
        {
            { "Token", token },
            { "User-Agent", _config["AadhaarService:User-Agent"] },
            { "AuthorisedKey", _config["AadhaarService:authKey"] }
        };

            var requestBody = new
            {
                id_number = request.aadhaar_number
            };

            try
            {
                var apiResponse = await _httpService
                    .PostAsync<object, AadharResponse>(apiUrl, requestBody, headers);

                if (apiResponse == null)
                    throw new AppException("External API failed");

                return apiResponse;
            }
            catch (Exception ex)
            {             
                throw; // middleware handle karega
            }
        }

        public async Task<ApiResponse<AadharVerifyResponse>> VerifyAadharAsync(AadhaarVerifyRequest request)
        {
            var response = new AadharVerifyResponse();
            //string apiUrl = "";
            ApiResponse<AadharVerifyResponse> apiResponse1 = new ApiResponse<AadharVerifyResponse>();
            try
            {
                //var config = _config.GetSection("Sprintverify");
                var token = _cL_JWTtoken.GenerateToken();

                // apiUrl = $"{config["AadhaarService:ApiBaseUrl"]}{config["AadhaarService:VerifyOtp"]}";
                string apiUrl = _config["AadhaarService:ApiBaseUrl"] + _config["AadhaarService:VerifyOtp"];
                var headers = new Dictionary<string, string>
        {
            { "Token", token },
            { "User-Agent", _config["AadhaarService:User-Agent"] },
            { "AuthorisedKey", _config["AadhaarService:authKey"] }
        };

                var body = new
                {
                    otp = request.otp,
                    client_id = request.TransactionId
                };

                var apiResponse = await _httpService.PostAsync<object, AadharVerifyResponse>(apiUrl, body, headers);
                if (apiResponse.statuscode == 200)
                {
                    apiResponse1.StatusCode = 200;
                    apiResponse1.Success = true;
                    apiResponse1.Data = response;
                    apiResponse1.Message = "Aadhar Verification Successfull";
                    return apiResponse1;
                }
                return apiResponse1;
            }
            catch (Exception Ex)
            {
                throw new NotImplementedException();
            }

        }


    }
}
