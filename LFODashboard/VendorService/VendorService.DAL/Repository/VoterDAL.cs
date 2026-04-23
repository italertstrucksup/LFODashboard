using Common.Core;
using HttpClientLib;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using VendorService.DAL.Interface;
using VendorService.Models;

namespace VendorService.DAL.Repository
{
    public class VoterDAL : IVoterDAL
    {
        private readonly IHttpService _httpService;
        private readonly IConfiguration _config;
        private readonly CL_JWTtokenGeneration _cL_JWTtoken;


        public VoterDAL(IHttpService httpService, IConfiguration config, CL_JWTtokenGeneration cL_JWTtoken)
        {
            _httpService = httpService;
            _config = config;
            _cL_JWTtoken = cL_JWTtoken;
        }
        public async Task<ApiResponse<VIDVerificationResponse>> VerifyVoterAsync(VIDVerificationRequest request)
        {
            ApiResponse<VIDVerificationResponse> apiResponse1 = new ApiResponse<VIDVerificationResponse>();
            string apiUrl = _config["SprintService:ApiBaseUrl"] + _config["SprintService:VIDVerify"];

            string token = _cL_JWTtoken.GenerateToken();

            var headers = new Dictionary<string, string>
            {
                { "Token", token },
                { "User-Agent", _config["SprintService:User-Agent"] },
                { "AuthorisedKey", _config["SprintService:authKey"] }
            };

            Random random = new Random();
            int code = random.Next(10000000, 99999999); // 8 digits

            var requestBody = new
            {
                refid = code,
                id_number = request.id_number
            };

            var apiResponse = await _httpService
                  .PostAsync<object, VIDVerificationResponse>(apiUrl, requestBody, headers);

            if (apiResponse == null)
                throw new AppException("External API failed");
            if (apiResponse.Statuscode == 200)
            {
                apiResponse1.StatusCode = 200;
                apiResponse1.Success = true;
                apiResponse1.Data = apiResponse;
                apiResponse1.Message = "Pan Verification Successfull";

            }
            return apiResponse1;
        }
    }
}
