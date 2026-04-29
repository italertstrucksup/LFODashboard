using Common.Core;
using MasterAPIServiceBL.Interface;
using MasterAPIServiceModel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MasterAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MasterController : ControllerBase
    {
        private readonly IMasterBL _masterBL;

        public MasterController(IMasterBL masterBL)
        {
            _masterBL = masterBL;
        }

        // GET api/master/operator/{id}
        [HttpGet("operator/{id}")]
        public async Task<IActionResult> GetOperatorMaster(int id)
        {
            var result = await _masterBL.GetOperatorMaster(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // GET api/master/document/{id}
        [HttpGet("document/{id}")]
        public async Task<IActionResult> GetDocumentMaster(int id)
        {
            var result = await _masterBL.GetDocumentMaster(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // GET api/master/vehicledetails/{action}
        [HttpGet("vehicledetails/{actionType}")]
        public async Task<IActionResult> GetVehicleDetailsMaster(string actionType, int? bodyId, int? tyreId)
        {
            var result = await _masterBL.GetVehicleDetailsMaster(actionType,bodyId,tyreId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // POST api/master/location-by-pincode
        [HttpGet("location-by-pincode")]
        public async Task<IActionResult> GetLocationByPincode(string  pincode)
        {
            if (string.IsNullOrEmpty(pincode))
            {
                return BadRequest("Pincode is required");
            }
            var result = await _masterBL.GetLocationByPincode(pincode);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // GET api/master/citymaster/{city}
        [HttpGet("citymaster")]
        public async Task<IActionResult> GetCityMaster(string? city = null)
        {
            var result = await _masterBL.GetCityMaster(city);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // GET api/master/kyc/{id}
        [HttpGet("kyc/{id}")]
        public async Task<IActionResult> GetKYCMaster(int id)
        {
            var result = await _masterBL.GetKYCMaster(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}