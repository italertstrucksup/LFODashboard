using Microsoft.AspNetCore.Mvc;
using ProfileService_LFO.BL.Interface;
using ProfileService_LFO.Model.Model;
using Common.Core;

namespace ProfileService_LFO.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IprofileDetails_BL _profileBL;

        public ProfileController(IprofileDetails_BL profileBL)
        {
            _profileBL = profileBL;
        }

       

        #region Update Profile
        [HttpPost("updateProfile")]
        public async Task<IActionResult> UpdateFleetOperator(UpdateFleetOperatorRequest request)
        {
            return Ok(await _profileBL.UpdateFleetOperator(request));
        }
        #endregion

        #region Update Profile by type
        [HttpPost("update/Profiletype")]
        public async Task<IActionResult> InsertFleetOperatorbyType(UpdateFleetOperatorRequest request)
        {
            return Ok(await _profileBL.InsertFleetOperatorbyType(request));
        }
        #endregion

        #region Add Lane
        [HttpPost("Addlanes")]
        public async Task<IActionResult> InsertPreferredLane(PreferredLaneRequest request)
        {
            return Ok(await _profileBL.InsertPreferredLane(request));
        }
        #endregion

        #region Upload Document
        [HttpPost("uploadDocument")]
        public async Task<IActionResult> InsertFleetOperatorDocument(UpdateDocumentRequest request)
        {
            return Ok(await _profileBL.InsertFleetOperatorDocument(request));
        }
        #endregion

        #region Add Truck
        [HttpPost("AddTruckstrucks")]
        public async Task<IActionResult> InsertTruckDetails([FromForm] TruckDetailsRequest request)
        {
            return Ok(await _profileBL.InsertTruckDetails(request));
        }
        #endregion

        #region KYC
        [HttpPost("Addkyc")]
        public async Task<IActionResult> InsertFleetOperatorKYC(KYCRequest request)
        {
            return Ok(await _profileBL.InsertFleetOperatorKYC(request));
        }
        #endregion
    }
}