using AuthServices_LFO.BL.Interface;
using AuthServices_LFO.DAL.Interface;
using AuthServices_LFO.Model.Models;
using Common.Core;
using DataAccessInterface;
using HttpClientLib;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<ApiResponse<TokenResponse>> UserLogin(LoginReq request)
        {
            try
            {
                var result = await _authDAL.ValidateUser(
                    request.UserName,
                    Helper.HashPassword(request.UserPassword)
                );

                // SAFETY CHECK
                if (result.Rows.Count == 0)
                {
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Something went wrong",
                        statusCode: 500
                    );
                }

                var row = result.Rows[0];

                var statusCode = Convert.ToInt32(row["StatusCode"]);

                if (statusCode == 0)
                {
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Invalid mobile number or password",
                        statusCode: 401
                    );
                }

                
                if (!Guid.TryParse(row["UserId"]?.ToString(), out Guid userId))
                {
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Invalid user data",
                        statusCode: 500
                    );
                }

                var isActive = Convert.ToBoolean(row["IsActive"]);

                var user = new UserEntity
                {
                    UserId = userId,
                    MobileNo = row["MobileNo"]?.ToString(),
                    AccessType = row["AccessType"]?.ToString(),
                    IsActive = isActive
                };

                var refreshExpiry = DateTime.UtcNow.AddDays(
                    int.Parse(_config["Jwt:RefreshTokenExpiryDays"])
                );

                var authRequest = new AuthenticationRequest
                {
                    UserName = request.UserName,
                    MobileNo = user.MobileNo,
                    AccessType = user.AccessType,
                    LoginId = user.UserId.ToString()
                };

                var tokenData = _jwtTokenHandler.GenrateJwtToken(authRequest);

                await _authDAL.SaveTokens(
                    user.UserId,
                    tokenData.RefreshToken,
                    refreshExpiry
                );

                return ApiResponse<TokenResponse>.SuccessResponse(
                    new TokenResponse
                    {
                        UserId = user.UserId,                
                        AccessType = user.AccessType,
                        ReferenceToken = tokenData.JwtToken,
                        RefreshToken = tokenData.RefreshToken,
                        Validity = tokenData.ExpiresIn
                    },
                    message: "Login successful"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<TokenResponse>.FailResponse(
                    ex.Message,
                    statusCode: 500
                );
            }
        }


        public async Task<ApiResponse<TokenResponse>> RefreshToken(LoginReq request)
        {
            try
            {
                var result = await _authDAL.GetAccessToken(request.RefreshToken);

                if (result.Rows.Count == 0)
                {
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Invalid refresh token",
                        statusCode: 401
                    );
                }

                var row = result.Rows[0];

                var statusCode = Convert.ToInt32(row["StatusCode"]);

                if (statusCode == 0)
                {
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Invalid or expired token",
                        statusCode: 401
                    );
                }

                var expiry = Convert.ToDateTime(row["TokenExpiry"]);

                if (expiry <= DateTime.UtcNow)
                {
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Refresh token expired",
                        statusCode: 401
                    );
                }

               
                if (!Guid.TryParse(row["UserId"]?.ToString(), out Guid userId))
                {
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Invalid user data",
                        statusCode: 500
                    );
                }

                var user = new UserEntity
                {
                    UserId = userId,
                    MobileNo = row["MobileNo"]?.ToString(),
                    AccessType = row["AccessType"]?.ToString(),
                    IsActive = Convert.ToBoolean(row["IsActive"])
                };

                var refreshExpiry = DateTime.UtcNow.AddDays(
                    int.Parse(_config["Jwt:RefreshTokenExpiryDays"])
                );

                var tokenData = _jwtTokenHandler.GetTokenByRefreshToken(request.RefreshToken);

                await _authDAL.SaveTokens(user.UserId, tokenData.RefreshToken, refreshExpiry);

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



        public async Task<ApiResponse<object>> RevokeToken(Guid userid, string refreshToken)
        {
            try
            {
                var result = await _authDAL.RevokeToken(userid, refreshToken);

                if (result.Rows.Count == 0)
                {
                    return ApiResponse<object>.FailResponse(
                        "Something went wrong",
                        statusCode: 500
                    );
                }

                var statusCode = Convert.ToInt32(result.Rows[0]["StatusCode"]);
                var message = result.Rows[0]["Message"]?.ToString();

                if (statusCode == 1)
                {
                    return ApiResponse<object>.SuccessResponse(
                        null,
                        message ?? "Logged out successfully"
                    );
                }

                return ApiResponse<object>.FailResponse(
                    message ?? "Logout failed",
                    statusCode: 400
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.FailResponse(
                    ex.Message,
                    statusCode: 500
                );
            }
        }

        public async Task<ApiResponse<SignupResponse>> SendLoginOtp(LoginOtpRequest request)
        {
            try
            {
                //  Check user
                var checkResult = await _authDAL.CheckUser(request.MobileNo);

                if (checkResult.Rows.Count == 0)
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Something went wrong",
                        statusCode: 500
                    );
                }

                var statusCode = Convert.ToInt32(checkResult.Rows[0]["StatusCode"]);

                //  User does NOT exist
                if (statusCode == 1)
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Mobile number not registered",
                        statusCode: 404
                    );
                }

                //  User blocked
                if (statusCode == 2)
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Account is blocked",
                        statusCode: 403
                    );
                }

                //  Generate OTP
                var otpResult = await _authDAL.GetOTP(request.MobileNo, "loginotp");

                if (otpResult.Rows.Count == 0)
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Failed to generate OTP",
                        statusCode: 500
                    );
                }

                var otp = otpResult.Rows[0]["OTP"]?.ToString();

                //  OTP NULL check 
                if (string.IsNullOrEmpty(otp))
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        "OTP generation failed",
                        statusCode: 500
                    );
                }

                
                var apiResponse = await CallOtpApi(request.MobileNo, otp, "loginotp");

                if (!apiResponse.Success)
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        apiResponse.Message,
                        statusCode: 502
                    );
                }

                
                return ApiResponse<SignupResponse>.SuccessResponse(
                    new SignupResponse
                    {
                        OTP = Convert.ToBase64String(Encoding.UTF8.GetBytes(otp))
                    },
                    message: "OTP sent successfully"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<SignupResponse>.FailResponse(
                    ex.Message,
                    statusCode: 500
                );
            }
        }


        public async Task<ApiResponse<TokenResponse>> LoginWithOtp(LoginOtpRequest request)
        {
            try
            {
                var result = await _authDAL.VerifyLoginOtp(
                    request.MobileNo,
                    request.OTP
                );

                if (result.Rows.Count == 0)
                {
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Invalid or expired OTP",
                        statusCode: 400
                    );
                }

                var row = result.Rows[0];

                var statusCode = Convert.ToInt32(row["StatusCode"]);

                if (statusCode == 0)
                {
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Invalid or expired OTP",
                        statusCode: 400
                    );
                }

                
                if (!Guid.TryParse(row["UserId"]?.ToString(), out Guid userId))
                {
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Invalid user data",
                        statusCode: 500
                    );
                }

                var isActive = Convert.ToBoolean(row["IsActive"]);

                if (!isActive)
                {
                    return ApiResponse<TokenResponse>.FailResponse(
                        "Account is deactive",
                        statusCode: 403
                    );
                }

                var user = new UserEntity
                {
                    UserId = userId,
                    MobileNo = row["MobileNo"]?.ToString(),
                    AccessType = row["AccessType"]?.ToString(),
                    IsActive = isActive
                };

                //  Token generate
                var refreshExpiry = DateTime.UtcNow.AddDays(
                    int.Parse(_config["Jwt:RefreshTokenExpiryDays"])
                );

                var authRequest = new AuthenticationRequest
                {
                    UserName = user.MobileNo,
                    MobileNo = user.MobileNo,
                    AccessType = user.AccessType,
                    LoginId = user.UserId.ToString()
                };

                var tokenData = _jwtTokenHandler.GenrateJwtToken(authRequest);

                // 🔹 Save token
                await _authDAL.SaveTokens(
                    user.UserId,
                    tokenData.RefreshToken,
                    refreshExpiry
                );

                return ApiResponse<TokenResponse>.SuccessResponse(
                    new TokenResponse
                    {
                        UserId = user.UserId,
                        AccessType = user.AccessType,
                        ReferenceToken = tokenData.JwtToken,
                        RefreshToken = tokenData.RefreshToken,
                        Validity = tokenData.ExpiresIn
                    },
                    message: "Login successful"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<TokenResponse>.FailResponse(
                    ex.Message,
                    statusCode: 500
                );
            }
        }




        public async Task<ApiResponse<SignupResponse>> SendSignupOtp(SignupOtpRequest request)
        {
            try
            {
                
                var checkResult = await _authDAL.CheckUser(request.MobileNo);

                if (checkResult.Rows.Count == 0)
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Something went wrong",
                        statusCode: 500
                    );
                }
               

                var statusCode = Convert.ToInt32(checkResult.Rows[0]["StatusCode"]);

                if (statusCode == 0)
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Mobile number already registered",
                        statusCode: 400
                    );
                }
                if (statusCode == 2) 
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Your account is blocked, please contact support.",
                        statusCode: 403
                    );
                }


                string otpType = "signup";

               
                var otpResult = await _authDAL.GetOTP( request.MobileNo, otpType);

                if (otpResult.Rows.Count == 0)
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Failed to generate OTP",
                        statusCode: 500
                    );
                }

                var otp = otpResult.Rows[0]["OTP"]?.ToString();

                //  Send OTP via API
                var apiResponse = await CallOtpApi(request.MobileNo, otp, otpType);

                if (!apiResponse.Success)
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        apiResponse.Message,
                        statusCode: 502
                    );
                }

                return ApiResponse<SignupResponse>.SuccessResponse(
                    new SignupResponse
                    {
                        OTP = Convert.ToBase64String(Encoding.UTF8.GetBytes(otp))
                    },
                    message: "OTP sent successfully"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<SignupResponse>.FailResponse(
                    ex.Message.ToString(),
                    statusCode: 500
                );
            }
        }


        public async Task<ApiResponse<SignupResponse>> UserRegister(SignupRequest request)
        {
            try
            {
                var hashedPassword = Helper.HashPassword(request.Password);

                var signupResult = await _authDAL.Signup(
                    request.MobileNo,
                    hashedPassword,
                    request.MobileNo
                );

                if (signupResult.Rows.Count == 0)
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Something went wrong",
                        statusCode: 500
                    );
                }

                var row = signupResult.Rows[0];

                //  StatusCode check 
                var statusCode = Convert.ToInt32(row["StatusCode"]);

                if (statusCode == 1)
                {
                    return ApiResponse<SignupResponse>.SuccessResponse(
                        null,
                        message: "User registered successfully"
                    );
                }

                return ApiResponse<SignupResponse>.FailResponse(
                    "User registration failed",
                    statusCode: 400
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<SignupResponse>.FailResponse(
                    ex.Message,
                    statusCode: 500
                );
            }
        }


      


        public async Task<ApiResponse<SignupResponse>> VerifySignupOTP(OTPVerifyRequest request)
        {
            try
            {
                var result = await _authDAL.VerifyOTP(
                    request.MobileNo,
                    request.OTP,
                    request.OTPType
                );

                if (result.Rows.Count == 0)
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Invalid or expired OTP",
                        statusCode: 400
                    );
                }

                var row = result.Rows[0];

                //  use StatusCode instead of Result
                var statusCode = Convert.ToInt32(row["StatusCode"]);

                if (statusCode == 1)
                {
                    return ApiResponse<SignupResponse>.SuccessResponse(
                        null,
                        message: "OTP verified successfully"
                    );
                }

                return ApiResponse<SignupResponse>.FailResponse(
                    "Invalid or expired OTP",
                    statusCode: 400
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<SignupResponse>.FailResponse(
                    ex.Message,
                    statusCode: 500
                );
            }
        }



        //----------------------RESET PASSWORD LOGIC-----------------------------

        public async Task<ApiResponse<SignupResponse>> SendResetOtp(ResetPasswordReq request)
        {
            try
            {
                // Check user
                var checkResult = await _authDAL.CheckUser(request.MobileNo);

                if (checkResult.Rows.Count == 0)
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Something went wrong",
                        statusCode: 500
                    );
                }

                var statusCode = Convert.ToInt32(checkResult.Rows[0]["StatusCode"]);

                // User does NOT exist
                if (statusCode == 1)
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Mobile number not registered",
                        statusCode: 404
                    );
                }

                // User blocked
                if (statusCode == 2)
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Account is blocked",
                        statusCode: 403
                    );
                }

                // Generate OTP (reset type)
                var otpResult = await _authDAL.GetOTP(request.MobileNo, "reset");

                if (otpResult.Rows.Count == 0)
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        "Failed to generate OTP",
                        statusCode: 500
                    );
                }

                var otp = otpResult.Rows[0]["OTP"]?.ToString();

                // OTP NULL check
                if (string.IsNullOrEmpty(otp))
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        "OTP generation failed",
                        statusCode: 500
                    );
                }

                // Send OTP
                var apiResponse = await CallOtpApi(request.MobileNo, otp, "reset");

                if (!apiResponse.Success)
                {
                    return ApiResponse<SignupResponse>.FailResponse(
                        apiResponse.Message,
                        statusCode: 502
                    );
                }

                // Return success
                return ApiResponse<SignupResponse>.SuccessResponse(
                    new SignupResponse
                    {
                        OTP = Convert.ToBase64String(Encoding.UTF8.GetBytes(otp))
                    },
                    message: "Reset OTP sent successfully"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<SignupResponse>.FailResponse(
                    ex.Message,
                    statusCode: 500
                );
            }
        }

        public async Task<ApiResponse<object>> ResetPassword(ResetPasswordReq request)
        {
            try
            {
                var result = await _authDAL.ResetPassword(
                    request.MobileNo,
                    request.OTP,
                    Helper.HashPassword(request.NewPassword)
                );

                if (result.Rows.Count == 0)
                {
                    return ApiResponse<object>.FailResponse(
                        "Something went wrong",
                        statusCode: 500
                    );
                }

                var row = result.Rows[0];

                var statusCode = Convert.ToInt32(row["StatusCode"]);
                var message = row["Message"]?.ToString();

                if (statusCode == 1)
                {
                    return ApiResponse<object>.SuccessResponse(
                        null,
                        message: "Password reset successful"
                    );
                }

                return ApiResponse<object>.FailResponse(
                    message ?? "Password reset failed",
                    statusCode: 400
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.FailResponse(
                    ex.Message,
                    statusCode: 500
                );
            }
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
                return ApiResponse<SignupResponse>.FailResponse("OTP API failed: " + ex.Message.ToString(), statusCode: 502);
            }
        }

    }
}
