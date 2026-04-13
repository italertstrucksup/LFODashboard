using VendorService.Models;

namespace VendorService.DAL.Interface
{
    public interface IWocomDAL
    {
        Task<WocomResponse> LoginOTP(string mobile, string otp);
        Task<WocomResponse> ResetOTP(string mobile, string otp);
        Task<WocomResponse> SignUpOTP(string mobile, string otp);
    }
}
