using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VendorService.BL.Interface;
using VendorService.Models;

namespace VendorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WocomController : ControllerBase
    {
        private readonly IWocomBL _wocom;
        public WocomController(IWocomBL wocom)
        {
            _wocom = wocom;
        }

        [HttpPost("OTPService")]
        public async Task<IActionResult> OTPService([FromBody] OtpRequest request)
        {
            var result = await _wocom.SendOTP(request.mobile, request.otp,request.otptype);
            return Ok(result);
        }
    }
}
