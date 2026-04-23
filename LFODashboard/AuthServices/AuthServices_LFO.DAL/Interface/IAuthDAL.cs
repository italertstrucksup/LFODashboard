using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AuthServices_LFO.DAL.Interface
{
    public interface IAuthDAL
    {
        Task<DataTable> ValidateUserAsync(string UserName, string Password);
        Task<DataTable> ValidateRefreshTokenAsync(string userName, string refreshToken);
        Task RevokeTokensAsync(int userId);
        Task SaveTokensAsync(int userId, string referenceToken, string refreshToken, DateTime validity);
        Task<DataTable> SaveLoginOtpAsync(string mobileNo, string otp, DateTime expiry);
        Task<DataTable> ValidateLoginOtpAsync(string mobileNo, string otp);

        Task<DataTable> CheckUserAsync(string mobileNo);
        Task<DataTable> SignupAsync(string mobileNo, string passwordHash, string createdBy);
        Task<DataTable> SaveOTPAsync(string mobileNo, string otp, string otpType, DateTime expiryTime);
        Task<DataTable> VerifyOTPAsync(string mobileNo, string otp, string otpType);
    }
}
