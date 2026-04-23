using Microsoft.AspNetCore.Mvc;
using ProfileService_LFO.BL.Interface;
using ProfileService_LFO.Model.Model;
using Common.Core;
using System.Data;


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

        #region Get Profile By Id
        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetProfileById(int userId)
        {
            var result = await _profileBL.GetProfileDetailsByIdAsync(userId);

            return Ok(ApiResponse<object>.SuccessResponse(result, "Profile fetched successfully", 200));
        }
        #endregion

        #region Update Profile
        [HttpPost("update")]
        public async Task<IActionResult> UpdateFleetOperator( UpdateFleetOperatorRequest request)
        {
            var result = await _profileBL.UpdateFleetOperator(request);

            return Ok(ApiResponse<object>.SuccessResponse(result, "Profile saved successfully", 200));
        }
        #endregion
        #region Update Profile by type
        [HttpPost("update/type")]
        public async Task<IActionResult> InsertFleetOperatorbyType([FromBody] UpdateFleetOperatorRequest request)
        {
            var result = await _profileBL.InsertFleetOperatorbyType(request);

            return Ok(ApiResponse<object>.SuccessResponse(result, "Profile saved successfully", 200));
        }
        #endregion
        #region Add Lane
        [HttpPost("lanes")]
        public async Task<IActionResult> InsertPreferredLane([FromBody] PreferredLaneRequest request)
        {
            var result = await _profileBL.InsertPreferredLane(request);

            return Ok(ApiResponse<object>.SuccessResponse(result, "Lane Added  successfully", 200));
        }
        #endregion

        #region Get Lanes
        [HttpGet("lanes/{loginId}")]
        public async Task<IActionResult> GetLanes(long loginId)
        {
            var result = await _profileBL.GetLanesAsync(loginId);

            return Ok(ApiResponse<object>.SuccessResponse(result, "Lane fetched successfully", 200));
        }
        #endregion

        //#region Add Truck
        //[HttpPost("trucks")]
        //public async Task<IActionResult> AddTruck([FromBody] TruckDetailsRequest request)
        //{
        //    var result = await _profileBL.AddTruckAsync(request);

        //    return Ok(ApiResponse<object>.SuccessResponse(result, "Truck Added  successfully", 200));
        //}
        //#endregion

        //[HttpGet("trucks/{profileId}")]
        //public async Task<IActionResult> GetTrucks(long profileId)
        //{
        //    var result = await _profileBL.GetTrucksAsync(profileId);

        //    return Ok(ApiResponse<object>.SuccessResponse(result, "Trucks fetched successfully", 200));
        //}
        [HttpPost("kyc")]
        public async Task<IActionResult> InsertFleetOperatorKYC(KYCRequest request)
        {
           

            var result = await _profileBL.InsertFleetOperatorKYC(request);

            

       return Ok(
                ApiResponse<object>.SuccessResponse(result, "kyc saved successfully", 200)
               );        
        }

        [HttpGet("kyc/{profileId}")]
        public async Task<IActionResult> GetKYC(long profileId)
        {
            var result = await _profileBL.GetKYCAsync(profileId);

            return Ok(ApiResponse<object>.SuccessResponse(result, "KYC fetched successfully", 200));
        }

        [HttpPost("upload")]
        public async Task<IActionResult> InsertFleetOperatorDocument( UpdateDocumentRequest request)
        {
            

            var result = await _profileBL.InsertFleetOperatorDocument(request);
            return Ok(
               ApiResponse<object>.SuccessResponse(result, "Documents uploaded successfully", 200)
           );

        }

        [HttpPost("kyc/documents")]
        public async Task<IActionResult> UpsertKYCDocuments( KYCDocumentRequest request)
        {
            var result = await _profileBL.UpsertKYCDocumentsAsync(request);

            return Ok(ApiResponse<object>.SuccessResponse(result, "KYC documents saved successfully", 200));
        }


    }
}