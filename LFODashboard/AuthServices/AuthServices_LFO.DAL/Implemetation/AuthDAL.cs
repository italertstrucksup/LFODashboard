using AuthServices_LFO.DAL.Interface;
using DataAccessInterface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AuthServices_LFO.DAL.Implemetation
{
    public class AuthDAL : IAuthDAL
    {
        private readonly string _connStr;
        private readonly IDataAccess _dataAccess;

        public AuthDAL(IConfiguration configuration, IDataAccess dataAccess)
        {
            _connStr = configuration.GetConnectionString("DefaultConnection");
            _dataAccess = dataAccess;
        }

        public async Task<DataTable> ValidateUserAsync(string UserName,string Password)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "VALIDATE_USER"),
                new SqlParameter("@MobileNo", UserName),
                new SqlParameter("@PasswordHash", Password)
            };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "LFO_SP_Login",
                parameters
            );

            return result;
        }

        public async Task<DataTable> ValidateRefreshTokenAsync(string userName, string refreshToken)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "VALIDATE_REFRESH_TOKEN"),
                new SqlParameter("@MobileNo", userName),
                new SqlParameter("@RefreshToken", refreshToken)
            };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "LFO_SP_Login",
                parameters
            );

            return result;
        }

        public async Task RevokeTokensAsync(int userId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "REVOKE_TOKENS"),
                new SqlParameter("@LoginId", userId)
            };

            await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "LFO_SP_Login",
                parameters
            );
        }

        public async Task SaveTokensAsync(int userId, string referenceToken, string refreshToken, DateTime validity)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action",         "SAVE_REFRESH_TOKEN"),
                new SqlParameter("@LoginId",         userId),
                new SqlParameter("@RefreshToken",   refreshToken),
                new SqlParameter("@Validity",       validity)
            };

            await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "LFO_SP_Login", parameters);
        }

        public async Task<DataTable> SaveLoginOtpAsync(string mobileNo, string otp, DateTime expiry)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@Action",   "SAVE_LOGIN_OTP"),
        new SqlParameter("@MobileNo", mobileNo),
        new SqlParameter("@OTP",      otp),
        new SqlParameter("@Validity", expiry)
    };

            return await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "LFO_SP_Login", parameters);
        }

        public async Task<DataTable> ValidateLoginOtpAsync(string mobileNo, string otp)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@Action",   "VALIDATE_LOGIN_OTP"),
        new SqlParameter("@MobileNo", mobileNo),
        new SqlParameter("@OTP",      otp)
    };

            return await _dataAccess.ExecuteStoredProcedureAsync(_connStr, "LFO_SP_Login", parameters);
        }


        // ─── SIGNUP ───────────────────────────────────────────

        public async Task<DataTable> CheckUserAsync(string mobileNo)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action",   "CHECK_USER"),
                new SqlParameter("@MobileNo", mobileNo)
            };

            return await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr, "LFO_SP_Signup", parameters);
        }

        public async Task<DataTable> SignupAsync(string mobileNo, string passwordHash, string createdBy)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action",       "SIGNUP"),
                new SqlParameter("@MobileNo",     mobileNo),
                new SqlParameter("@PasswordHash", passwordHash),
                new SqlParameter("@CreatedBy",    createdBy)
            };

            return await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr, "LFO_SP_Signup", parameters);
        }

        public async Task<DataTable> SaveOTPAsync(string mobileNo, string otp, string otpType, DateTime expiryTime)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action",     "SAVE_OTP"),
                new SqlParameter("@MobileNo",   mobileNo),
                new SqlParameter("@OTP",        otp),
                new SqlParameter("@OTPType",    otpType),
                new SqlParameter("@ExpiryTime", expiryTime)
            };

            return await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr, "LFO_SP_Signup", parameters);
        }

        public async Task<DataTable> VerifyOTPAsync(string mobileNo, string otp, string otpType)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action",   "VERIFY_OTP"),
                new SqlParameter("@MobileNo", mobileNo),
                new SqlParameter("@OTP",      otp),
                new SqlParameter("@OTPType",  otpType)
            };

            return await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr, "LFO_SP_Signup", parameters);
        }


    }
}
