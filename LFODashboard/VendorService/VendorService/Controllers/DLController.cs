using Common.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VendorService.BL.Interface;
using VendorService.Models;

namespace VendorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DLController : ControllerBase
    {
        private readonly IDLService _dlservice;

        public DLController(IDLService dlservice)
        {
            _dlservice = dlservice;
        }


        [HttpPost]
        [Route("verify-dl")]
        public async Task<IActionResult> VerifyDrivingLicence(ApiRequest< DrivingLicenceRequest> drivingLicenceRequest)
        {
            var result = await _dlservice.VerifyDrivingLicence(drivingLicenceRequest.Data);

            return Ok(result);
        }
    }
}
