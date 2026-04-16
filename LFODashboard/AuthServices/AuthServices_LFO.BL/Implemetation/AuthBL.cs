using AuthServices_LFO.BL.Interface;
using AuthServices_LFO.DAL.Interface;
using AuthServices_LFO.Model.Models;
using HttpClientLib;
using DataAccessInterface;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

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
        public async Task<TokenResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var result = await _authDAL.ValidateUserAsync(request.UserName, Helper.HashPassword(request.UserPassword));

                if (result.Rows.Count == 0)
                    return new TokenResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid mobile number or password"
                    };

                var row = result.Rows[0];
                var isActive = Convert.ToBoolean(row["IsActive"]);

                if (!isActive)
                    return new TokenResponse
                    {
                        IsSuccess = false,
                        Message = "Account is Deactive"
                    };
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
                    MobileNo= user.MobileNo,
                    AccessType = user.AccessType,
                    LoginId= user.UserId.ToString()
                };

                AuthenticationResponse tokenData = _jwtTokenHandler.GenrateJwtToken(req);

                await _authDAL.SaveTokensAsync(user.UserId, tokenData.JwtToken, tokenData.RefreshToken, refreshExpiry);

                return new TokenResponse
                {
                    IsSuccess = true,
                    Message = "Login successful",
                    ReferenceToken = tokenData.JwtToken,
                    RefreshToken = tokenData.RefreshToken,
                    Validity = tokenData.ExpiresIn
                };
            }
            catch (Exception ex)
            {
                return new TokenResponse
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }

        }

        public async Task<TokenResponse> RefreshTokenAsync(LoginRequest request)
        {
            try
            {

                var result = await _authDAL.ValidateRefreshTokenAsync(request.UserName, request.RefreshToken);

                if (result.Rows.Count == 0)
                    return new TokenResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid refresh token"
                    };

                var row = result.Rows[0];
                var validity = Convert.ToDateTime(row["TokenExpiry"]);

                if (validity <= DateTime.UtcNow)
                    return new TokenResponse
                    {
                        IsSuccess = false,
                        Message = "Refresh token expired, please login again"
                    };
                var user = new UserEntity
                {
                    UserId = Convert.ToInt32(row["LoginId"]),
                    MobileNo = row["MobileNo"].ToString(),
                    AccessType = row["AccessType"].ToString(),
                    IsActive = Convert.ToBoolean(row["IsActive"])
                };

                var refreshExpiry = DateTime.UtcNow.AddDays(int.Parse(_config["Jwt:RefreshTokenExpiryDays"]));

                AuthenticationResponse tokenData = _jwtTokenHandler.GetTokenByRefreshToken(request.RefreshToken);

                // 4. DB mein save karo
                await _authDAL.SaveTokensAsync(user.UserId, tokenData.JwtToken, tokenData.RefreshToken, refreshExpiry);

                return new TokenResponse
                {
                    IsSuccess = true,
                    Message = "Token refreshed successfully",
                    ReferenceToken = tokenData.JwtToken,
                    RefreshToken = tokenData.RefreshToken,
                    Validity = tokenData.ExpiresIn
                };
            }
            catch (Exception ex)
            {
                return new TokenResponse
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }

        }

        public async Task<bool> RevokeTokenAsync(int userId)
        {
            try
            {
                await _authDAL.RevokeTokensAsync(userId);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<SignupResponse> SendSignupOtpAsync(SignupRequest request)
        {
            try
            {
                var checkResult = await _authDAL.CheckUserAsync(request.MobileNo);

                if (checkResult.Rows.Count > 0)
                    return new SignupResponse
                    {
                        IsSuccess = false,
                        Message = "Mobile number already registered, please login"
                    };


                //  OTP generate karo
                var otp = GenerateOTP();
                var expiry = DateTime.Now.AddSeconds(60);

                await _authDAL.SaveOTPAsync(request.MobileNo, otp, "SIGNUPOTP", expiry);

                var apiResponse = await CallOtpApi(request.MobileNo, otp, "signup");

                if (!apiResponse.IsSuccess)
                    return apiResponse;

                return new SignupResponse
                {
                    IsSuccess = true,
                    Message = "OTP sent successfully",
                    //OTP = otp
                    OTP = apiResponse.OTP
                };
            }
            catch (Exception ex)
            {
                return new SignupResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<SignupResponse> SignupAsync(SignupRequest request)
        {
            try
            {
                var hashedPassword = Helper.HashPassword(request.Password);

                var signupResult = await _authDAL.SignupAsync(request.MobileNo, hashedPassword, request.MobileNo);

                if (signupResult.Rows.Count > 0 )
                    return new SignupResponse
                    {
                        IsSuccess = true,
                        Message = signupResult.Rows[0]["result"].ToString()
                    };
                else
                    return new SignupResponse
                    {
                        IsSuccess = false,
                        Message = "Somthing went Wrong"
                    };
            }
            catch (Exception ex)
            {
                return new SignupResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        // ─── VERIFY OTP ───────────────────────────────────────
        public async Task<SignupResponse> VerifyOTPAsync(OTPVerifyRequest request)
        {
            try
            {
                var result = await _authDAL.VerifyOTPAsync(request.MobileNo, request.OTP, request.OTPType);

                if (result.Rows.Count == 0)
                    return new SignupResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid or expired OTP"
                    };

                var dbResult = result.Rows[0]["Result"].ToString();

                if (dbResult == "SUCCESS")
                {
                    
                    return new SignupResponse
                    {
                        IsSuccess = true,
                        Message = "OTP verified successfully, please login"
                    };

                }

                return new SignupResponse
                {
                    IsSuccess = false,
                    Message = "Invalid or expired OTP"
                };

            }
            catch (Exception ex)
            {
                return new SignupResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        // ─── HELPER ───────────────────────────────────────────
        private string GenerateOTP()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString();
        }

        //------------------------VENDOR OTP API CALL-----------------------------
        
        private async Task<SignupResponse> CallOtpApi(string mobile, string otp, string otpType)
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

                var response = await _httpService.PostAsync<OtpVendorRequest, SignupResponse>(url, request);

                return response;
            }
            catch (Exception ex)
            {
                return new SignupResponse
                {
                    IsSuccess = false,
                    Message = "OTP API failed: " + ex.Message
                };
            }
        }


    }
}
