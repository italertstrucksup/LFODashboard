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

      
    }
}
