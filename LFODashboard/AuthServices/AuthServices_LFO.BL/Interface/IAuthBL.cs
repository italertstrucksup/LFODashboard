
using AuthServices_LFO.Model.Models;
using Common.Core;

namespace AuthServices_LFO.BL.Interface
{
    public interface IAuthBL
    {
        Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request);
        Task<ApiResponse<TokenResponse>> RefreshTokenAsync(LoginRequest request);
        Task<ApiResponse<object>> RevokeTokenAsync(int userId);

        Task<ApiResponse<SignupResponse>> SendLoginOtpAsync(LoginOtpRequest request);
        Task<ApiResponse<TokenResponse>> LoginWithOtpAsync(LoginOtpRequest request);


        Task<ApiResponse<SignupResponse>> SendSignupOtpAsync(SignupRequest request);

        Task<ApiResponse<SignupResponse>> SignupAsync(SignupRequest request);
        Task<ApiResponse<SignupResponse>> VerifyOTPAsync(OTPVerifyRequest request);
        


        }
}
