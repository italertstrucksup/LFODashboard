
using AuthServices_LFO.Model.Models;
using Common.Core;

namespace AuthServices_LFO.BL.Interface
{
    public interface IAuthBL
    {
        Task<ApiResponse<TokenResponse>> UserLogin(LoginReq request);
        Task<ApiResponse<TokenResponse>> RefreshToken(LoginReq request);
        Task<ApiResponse<object>> RevokeToken(Guid userId, string refreshToken);

        Task<ApiResponse<SignupResponse>> SendLoginOtp(LoginOtpRequest request);
        Task<ApiResponse<TokenResponse>> LoginWithOtp(LoginOtpRequest request);


        Task<ApiResponse<SignupResponse>> SendSignupOtp(SignupOtpRequest request);

        Task<ApiResponse<SignupResponse>> UserRegister(SignupRequest request);
        Task<ApiResponse<SignupResponse>> VerifySignupOTP(OTPVerifyRequest request);

        Task<ApiResponse<SignupResponse>> SendResetOtp(ResetPasswordOTPRequest request);
        Task<ApiResponse<object>> VerifyOTPResetPassword(VerifyOTPReq request);

        Task<ApiResponse<object>> ResetPassword(ResetPasswordReq request);




        }
}
