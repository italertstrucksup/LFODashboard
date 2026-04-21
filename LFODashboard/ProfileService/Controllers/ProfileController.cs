using Microsoft.AspNetCore.Mvc;
using ProfileService_LFO.BL.Interface;
using ProfileService_LFO.Model.Models;
using Common.Core;


namespace ProfileService_LFO.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IprofileDetails_BL _profileBL;

        public ProfileController(IprofileDetails_BL profileBL)
        {
            _profileBL = profileBL;
        }

        #region Get Profile By Id
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfileById(int userId)
        {
            var result = await _profileBL.GetProfileDetailsByIdAsync(userId);

            if (result == null)
                return NotFound("Profile not found");

            return Ok(result); // returning raw data (DataTable or object)


        }
        #endregion

        #region Upsert Profile
        [HttpPost]
        public async Task<IActionResult> UpsertProfile([FromBody] UpsertProfileRequest request)
        {
            var result = await _profileBL.UpsertProfileAsync(request);

            return Ok(result); // could be bool or DataTable based on your BL
        }
        #endregion
        #region Add Lane
        [HttpPost]
        public async Task<IActionResult> AddLane([FromBody] PreferredLaneRequest request)
        {
            await _profileBL.AddLaneAsync(request);

            return Ok("Lane added successfully");
        }
        #endregion

        #region Get Lanes
        [HttpGet("{loginId}")]
        public async Task<IActionResult> GetLanes(long loginId)
        {
            var result = await _profileBL.GetLanesAsync(loginId);

            return Ok(result);
        }
        #endregion

        #region Add Truck
        [HttpPost]
        public async Task<IActionResult> AddTruck([FromBody] TruckDetailsRequest request)
        {
            var result = await _profileBL.AddTruckAsync(request);

            return Ok("Truck added successfully");
        }
        #endregion

        #region Get Trucks
        [HttpGet("{profileId}")]
        public async Task<IActionResult> GetTrucks(long profileId)
        {
            var result = await _profileBL.GetTrucksAsync(profileId);

            return Ok(result);
        }
        #endregion
        #region Upsert KYC
        [HttpPost]
        public async Task<IActionResult> UpsertKYC([FromBody] KYCRequest request)
        {
            var result = await _profileBL.UpsertKYCAsync(request);

            if (!result)
                return BadRequest("Failed to save KYC");

            return Ok("KYC saved successfully");
        }
        #endregion

        #region Get KYC
        [HttpGet("{profileId}")]
        public async Task<IActionResult> GetKYC(long profileId)
        {
            var result = await _profileBL.GetKYCAsync(profileId);

            return Ok(result);
        }
        #endregion

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocuments([FromBody] UpsertDocumentRequest request)
        {
            var result = await _profileBL.UpsertDocumentsAsync(request);

            if (!result)
                return BadRequest("Failed to upload documents");

            return Ok("Documents uploaded successfully");
        }

        [HttpPost]
        public async Task<IActionResult> UpsertKYCDocuments([FromBody] KYCDocumentRequest request)
        {
            var result = await _profileBL.UpsertKYCDocumentsAsync(request);

            if (!result)
                return BadRequest("Failed to save KYC documents");

            return Ok("KYC documents saved successfully");
        }


    }
}