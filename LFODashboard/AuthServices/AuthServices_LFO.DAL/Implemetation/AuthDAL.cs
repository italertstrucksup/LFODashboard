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

        //---------LOGIN------------------
        public async Task<DataTable> ValidateUser(string MobileNumber,string Password)
        {
            var parameters = new List<SqlParameter>
            {
                
                new SqlParameter("@MobileNo", MobileNumber),
                new SqlParameter("@PasswordHash", Password)
            };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "usp_validateuser",
                parameters
            );

            return result;
        }

        public async Task SaveTokens(Guid userid, string refreshToken, DateTime expiryRefreshToken)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", userid),
                new SqlParameter("@RefreshToken", refreshToken),
                new SqlParameter("@ExpiryRefreshToken", expiryRefreshToken)
            };

            await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr, "usp_savetokens", parameters);
        }


        public async Task<DataTable> GetAccessToken( string refreshToken)
        {
            var parameters = new List<SqlParameter>
            {

               
                new SqlParameter("@RefreshToken", refreshToken)
            };

            var result = await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "usp_getnewaccesstoken",
                parameters
            );

            return result;
        }

        public async Task<DataTable> RevokeToken(Guid userid, string refreshToken)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@UserId", userid),
        new SqlParameter("@RefreshToken", refreshToken)
    };

            return await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "usp_revoketoken",
                parameters
            );
        }




        // ─── SIGNUP ───────────────────────────────────────────

        public async Task<DataTable> CheckUser(string mobileNo)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MobileNo", mobileNo)
            };

            return await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr, "usp_checkuser", parameters);
        }


        public async Task<DataTable> Signup(string mobileNo, string passwordHash, string createdBy)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MobileNo",     mobileNo),
                new SqlParameter("@PasswordHash", passwordHash),
                new SqlParameter("@CreatedBy",    createdBy)
            };

            return await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr, "usp_signup", parameters);
        }


        
        //---------------------OTP SAVE AND VERIFY-------------------------
        public async Task<DataTable> GetOTP( string mobileNo, string otpType)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserId", DBNull.Value),
                new SqlParameter("@MobileNo",   mobileNo),
                new SqlParameter("@OTPType",    otpType)
            };

            return await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr, "usp_generateotp", parameters);
        }

        public async Task<DataTable> VerifyOTP(string mobileNo, string otp, string otpType)
        {
            var parameters = new List<SqlParameter>
            {
                
                new SqlParameter("@MobileNo", mobileNo),
                new SqlParameter("@OTP",      otp),
                new SqlParameter("@OTPType",  otpType)
            };

            return await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr, "usp_verifyotp", parameters);
        }


        public async Task<DataTable> VerifyLoginOtp(string mobileNo, string otp)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@MobileNo", mobileNo),
        new SqlParameter("@OTP", otp)
    };

            return await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "usp_verifyotplogin",
                parameters
            );
        }


        public async Task<DataTable> VerifyOTPResetPassword(string mobileNo, string otp)
        {
            var parameters = new List<SqlParameter>
        {
            new SqlParameter("@MobileNo", mobileNo),
            new SqlParameter("@OTP", otp)
        };

            return await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "usp_verifyresetotp",
                parameters
            );
        }


        //------------------RESET PASSWORD-----------------------

        public async Task<DataTable> ResetPassword(string mobileNo, string newPasswordHash)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@MobileNo", mobileNo),
                new SqlParameter("@NewPasswordHash", newPasswordHash)
            };

            return await _dataAccess.ExecuteStoredProcedureAsync(
                _connStr,
                "usp_resetpassword",
                parameters
            );
        }


    }
}
