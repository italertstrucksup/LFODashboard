using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AuthServices_LFO.DAL.Interface
{
    public interface IAuthDAL
    {
        Task<DataTable> ValidateUser(string MobileNumber, string Password);
        Task<DataTable> GetAccessToken( string refreshToken);
        Task<DataTable> RevokeToken(Guid userid, string refreshToken);
        Task SaveTokens(Guid userid, string refreshToken, DateTime expiryRefreshToken);
        

        Task<DataTable> CheckUser(string mobileNo);
        Task<DataTable> Signup(string mobileNo, string passwordHash, string createdBy);
        Task<DataTable> GetOTP( string mobileNo, string otpType);
        Task<DataTable> VerifyOTP( string mobileNo, string otp, string otpType);
        Task<DataTable> VerifyLoginOtp(string mobileNo, string otp);

        Task<DataTable> ResetPassword(string mobileNo, string otp, string newPasswordHash);
    }
}
