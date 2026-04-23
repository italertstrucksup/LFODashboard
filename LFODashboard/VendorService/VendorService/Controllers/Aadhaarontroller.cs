using Common.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VendorService.BL.Interface;
using VendorService.Models;

namespace VendorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Aadhaarontroller : ControllerBase
    {
        private readonly IAadharService _aadhar;

        public Aadhaarontroller(IAadharService aadhar)
        {
            _aadhar = aadhar;
        }


        [HttpPost]
        [Route("send_otp_aadhaar")]
        public async Task<IActionResult> SendAadharOTPAsync([FromBody] ApiRequest<AadhaarRequest> request)
        {
            var result = await _aadhar.SendAadharOTPAsync(request.Data);

            return Ok(result);
        }

        [HttpPost]
        [Route("verify_aadhaar")]
        public async Task<IActionResult> VerifyAadharAsync([FromBody] ApiRequest<AadhaarVerifyRequest> request)
        {
            var result = await _aadhar.VerifyAadharAsync(request.Data);

            return Ok(result);
        }
    }
}
