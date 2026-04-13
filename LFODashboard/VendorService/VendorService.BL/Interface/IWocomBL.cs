using VendorService.Models;

namespace VendorService.BL.Interface
{
    public interface IWocomBL
    {
        Task<WocomResponse> SendOTP(string mobile, string otp, string otptype);
    }
}
