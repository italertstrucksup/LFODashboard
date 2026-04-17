using AuthServices_LFO.BL.Interface;
using AuthServices_LFO.DAL.Interface;
using AuthServices_LFO.Model.Models;
using Common.Core;
using DataAccessInterface;
using HttpClientLib;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Encodings.Web;

namespace AuthServices_LFO.BL.Implemetation
{
    public class AuthBL : IAuthBL
    {
        private readonly IConfiguration _config;
        private readonly IAuthDAL _authDAL;
        private readonly IDataAccess _dataAccess;
        private readonly IHttpService _httpService;
        private readonly JwtTokenHandler _jwtTokenHandler;
        public AuthBL(IConfiguration config, IDataAccess dataAccess, JwtTokenHandler jwtTokenHandler, IAuthDAL authDAL, IHttpService httpService)
        {
            _config = config;
            _dataAccess = dataAccess;
            _jwtTokenHandler = jwtTokenHandler;
            _authDAL = authDAL;
            _httpService = httpService;
        }

        //-------------------------Login-----------------------------------

        public async Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                var result = await _authDAL.ValidateUserAsync(request.UserName, Helper.HashPassword(request.UserPassword));

                if (result.Rows.Count == 0)
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Invalid mobile number or password",
                        statusCode: 401
                    );

                var row = result.Rows[0];
                var isActive = Convert.ToBoolean(row["IsActive"]);

                if (!isActive)
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Account is deactive",
                        statusCode: 403
                    );

                var user = new UserEntity
                {
                    UserId = Convert.ToInt32(row["LoginId"]),
                    MobileNo = row["MobileNo"].ToString(),
                    AccessType = row["AccessType"].ToString(),
                    IsActive = isActive
                };

                var refreshExpiry = DateTime.UtcNow.AddDays(int.Parse(_config["Jwt:RefreshTokenExpiryDays"]));

                AuthenticationRequest req = new AuthenticationRequest
                {
                    UserName = request.UserName,
                    MobileNo = user.MobileNo,
                    AccessType = user.AccessType,
                    LoginId = user.UserId.ToString()
                };

                AuthenticationResponse tokenData = _jwtTokenHandler.GenrateJwtToken(req);

                await _authDAL.SaveTokensAsync(user.UserId, tokenData.JwtToken, tokenData.RefreshToken, refreshExpiry);

                return ApiResponse<TokenResponse>.SuccessResponse(
                    new TokenResponse
                    {
                        ReferenceToken = tokenData.JwtToken,
                        RefreshToken = tokenData.RefreshToken,
                        Validity = tokenData.ExpiresIn
                    },
                    message: "Login successful"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<TokenResponse>.FailResponse(ex.Message, statusCode: 500);
            }
        }


     

        public async Task<ApiResponse<TokenResponse>> RefreshTokenAsync(LoginRequest request)
        {
            try
            {
                var result = await _authDAL.ValidateRefreshTokenAsync(request.UserName, request.RefreshToken);

                if (result.Rows.Count == 0)
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Invalid refresh token",
                        statusCode: 401
                    );

                var row = result.Rows[0];
                var validity = Convert.ToDateTime(row["TokenExpiry"]);

                if (validity <= DateTime.UtcNow)
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Refresh token expired, please login again",
                        statusCode: 401
                    );

                var user = new UserEntity
                {
                    UserId = Convert.ToInt32(row["LoginId"]),
                    MobileNo = row["MobileNo"].ToString(),
                    AccessType = row["AccessType"].ToString(),
                    IsActive = Convert.ToBoolean(row["IsActive"])
                };

                var refreshExpiry = DateTime.UtcNow.AddDays(int.Parse(_config["Jwt:RefreshTokenExpiryDays"]));

                AuthenticationResponse tokenData = _jwtTokenHandler.GetTokenByRefreshToken(request.RefreshToken);

                await _authDAL.SaveTokensAsync(user.UserId, tokenData.JwtToken, tokenData.RefreshToken, refreshExpiry);

                return ApiResponse<TokenResponse>.SuccessResponse(
                    new TokenResponse
                    {
                        ReferenceToken = tokenData.JwtToken,
                        RefreshToken = tokenData.RefreshToken,
                        Validity = tokenData.ExpiresIn
                    },
                    message: "Token refreshed successfully"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<TokenResponse>.FailResponse(ex.Message, statusCode: 500);
            }
        }

      

        public async Task<ApiResponse<object>> RevokeTokenAsync(int userId)
        {
            try
            {
                await _authDAL.RevokeTokensAsync(userId);

                return ApiResponse<object>.SuccessResponse(
                    null,
                    message: "Logged out successfully"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.FailResponse(ex.Message, statusCode: 500);
            }
        }

        public async Task<ApiResponse<SignupResponse>> SendLoginOtpAsync(LoginOtpRequest request)
        {
            try
            {
                // User exist karta hai ya nahi
                var checkResult = await _authDAL.CheckUserAsync(request.MobileNo);

                if (checkResult.Rows.Count == 0)
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Mobile number not registered, please signup",
                        statusCode: 404
                    );

                var isActive = Convert.ToBoolean(checkResult.Rows[0]["IsActive"]);
                if (!isActive)
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Account is deactive",
                        statusCode: 403
                    );

                var otp = GenerateOTP();
                var expiry = DateTime.Now.AddSeconds(60);

                await _authDAL.SaveLoginOtpAsync(request.MobileNo, otp, expiry);

                var apiResponse = await CallOtpApi(request.MobileNo, otp, "loginotp");

                if (!apiResponse.Success)
                    return ApiResponse<SignupResponse>.FailResponse(
                        apiResponse.Message,
                        statusCode: 502
                    );

                return ApiResponse<SignupResponse>.SuccessResponse(
                    new SignupResponse { OTP = Convert.ToBase64String(Encoding.UTF8.GetBytes(apiResponse.Data?.OTP)) },
                    message: "OTP sent successfully"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<SignupResponse>.FailResponse(ex.Message, statusCode: 500);
            }
        }


        public async Task<ApiResponse<TokenResponse>> LoginWithOtpAsync(LoginOtpRequest request)
        {
            try
            {
                var result = await _authDAL.ValidateLoginOtpAsync(request.MobileNo, request.OTP);

                if (result.Rows.Count == 0)
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Invalid or expired OTP",
                        statusCode: 400
                    );

                var row = result.Rows[0];

                if (row["Result"].ToString() == "FAILED")
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Invalid or expired OTP",
                        statusCode: 400
                    );

                var isActive = Convert.ToBoolean(row["IsActive"]);
                if (!isActive)
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Account is deactive",
                        statusCode: 403
                    );

                var user = new UserEntity
                {
                    UserId = Convert.ToInt32(row["LoginId"]),
                    MobileNo = row["MobileNo"].ToString(),
                    AccessType = row["AccessType"].ToString(),
                    IsActive = isActive
                };

                var refreshExpiry = DateTime.UtcNow.AddDays(int.Parse(_config["Jwt:RefreshTokenExpiryDays"]));

                AuthenticationRequest req = new AuthenticationRequest
                {
                    UserName = user.MobileNo,
                    MobileNo = user.MobileNo,
                    AccessType = user.AccessType,
                    LoginId = user.UserId.ToString()
                };

                AuthenticationResponse tokenData = _jwtTokenHandler.GenrateJwtToken(req);

                await _authDAL.SaveTokensAsync(user.UserId, tokenData.JwtToken, tokenData.RefreshToken, refreshExpiry);

                return ApiResponse<TokenResponse>.SuccessResponse(
                    new TokenResponse
                    {
                        ReferenceToken = tokenData.JwtToken,
                        RefreshToken = tokenData.RefreshToken,
                        Validity = tokenData.ExpiresIn
                    },
                    message: "Login successful"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<TokenResponse>.FailResponse(ex.Message, statusCode: 500);
            }
        }


        //------------------------SIGNUP-------------------------

        public async Task<ApiResponse<SignupResponse>> SendSignupOtpAsync(SignupRequest request)
        {
            try
            {
                var checkResult = await _authDAL.CheckUserAsync(request.MobileNo);

                if (checkResult.Rows.Count > 0)
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Mobile number already registered, please login",
                        statusCode: 400
                    );

                var otp = GenerateOTP();
                var expiry = DateTime.Now.AddSeconds(60);
                string otpType = "signup";

                await _authDAL.SaveOTPAsync(request.MobileNo, otp, otpType, expiry);

                var apiResponse = await CallOtpApi(request.MobileNo, otp, otpType);

                if (!apiResponse.Success)
                    return ApiResponse<SignupResponse>.FailResponse(
                        apiResponse.Message,
                        statusCode: 502
                    );

                return ApiResponse<SignupResponse>.SuccessResponse(
                    new SignupResponse { OTP = Convert.ToBase64String(Encoding.UTF8.GetBytes(apiResponse.Data?.OTP)) },
                    message: "OTP sent successfully"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<SignupResponse>.FailResponse(ex.Message, statusCode: 500);
            }
        }



        public async Task <ApiResponse<SignupResponse>> SignupAsync(SignupRequest request)
        {
            try
            {
                var hashedPassword = Helper.HashPassword(request.Password);

                var signupResult = await _authDAL.SignupAsync(request.MobileNo, hashedPassword, request.MobileNo);

              
                if (signupResult.Rows.Count > 0)
                    return ApiResponse<SignupResponse>.SuccessResponse(
                        null,
                        message: signupResult.Rows[0]["result"].ToString()
                    );
                else
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Something went wrong",
                        statusCode: 500
                    );
            }
            catch (Exception ex)
            {
                return ApiResponse<SignupResponse>.FailResponse(ex.Message, statusCode: 500);
            }
        }

        // ─── VERIFY OTP ───────────────────────────────────────

        public async Task<ApiResponse<SignupResponse>> VerifyOTPAsync(OTPVerifyRequest request)
        {
            try
            {
                var result = await _authDAL.VerifyOTPAsync(request.MobileNo, request.OTP, request.OTPType);

                if (result.Rows.Count == 0)
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Invalid or expired OTP",
                        statusCode: 400
                    );

                var dbResult = result.Rows[0]["Result"].ToString();

                if (dbResult == "SUCCESS")
                    return ApiResponse<SignupResponse>.SuccessResponse(
                        null,
                        message: "OTP verified successfully, please login"
                    );

                return ApiResponse<SignupResponse>.FailResponse(
                    "Invalid or expired OTP",
                    statusCode: 400
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<SignupResponse>.FailResponse(ex.Message, statusCode: 500);
            }
        }



        // ─── HELPER ───────────────────────────────────────────
        private string GenerateOTP()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString();
        }

        //------------------------VENDOR OTP API CALL-----------------------------

        private async Task<ApiResponse<SignupResponse>> CallOtpApi(string mobile, string otp, string otpType)
        {
            try
            {
                var url = _config["WocomSettings:OtpApiUrl"];

                var request = new OtpVendorRequest
                {
                    mobile = mobile,
                    otp = otp,
                    otptype = otpType
                };

                var response = await _httpService.PostAsync<OtpVendorRequest, OtpVendorResponse>(url, request);

                if (response.StatusCode == 200)
                    return ApiResponse<SignupResponse>.SuccessResponse(
                        new SignupResponse { OTP = otp },
                        message: response.Message
                    );
                else
                    return ApiResponse<SignupResponse>.FailResponse(response.Message, statusCode: response.StatusCode);
            }
            catch (Exception ex)
            {
                return ApiResponse<SignupResponse>.FailResponse("OTP API failed: " + ex.Message, statusCode: 502);
            }
        }

    }
}
