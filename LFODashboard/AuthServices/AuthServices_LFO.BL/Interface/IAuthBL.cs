
using AuthServices_LFO.Model.Models;

namespace AuthServices_LFO.BL.Interface
{
    public interface IAuthBL
    {
        Task<TokenResponse> LoginAsync(LoginRequest request);
        Task<TokenResponse> RefreshTokenAsync(LoginRequest request);
        Task<bool> RevokeTokenAsync(int userId);

        Task<SignupResponse> SendSignupOtpAsync(SignupRequest request);
        Task<SignupResponse> SignupAsync(SignupRequest request);
        Task<SignupResponse> VerifyOTPAsync(OTPVerifyRequest request);
    }
}
