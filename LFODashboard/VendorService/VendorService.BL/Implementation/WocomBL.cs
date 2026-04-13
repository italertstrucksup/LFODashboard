using VendorService.BL.Interface;
using VendorService.DAL.Interface;
using VendorService.Models;

namespace VendorService.BL.Implementation
{
    public class WocomBL : IWocomBL
    {
        private readonly IWocomDAL _wocomDAL;

        public WocomBL(IWocomDAL wocomDAL)
        {
            _wocomDAL = wocomDAL;
        }

        public async Task<WocomResponse> SendOTP(string mobile, string otp, string otptype)
        {
            if (otptype == "loginotp")
            {
                return await _wocomDAL.LoginOTP(mobile, otp);
            }
            else if (otptype == "reset")
            {
                return await _wocomDAL.ResetOTP(mobile, otp);
            }
            else if (otptype == "signup")
            {
                return await _wocomDAL.SignUpOTP(mobile, otp);
            }

            return new WocomResponse { StatusCode = 400, Message = "Invalid OTP Type" };
        }
    }
}
