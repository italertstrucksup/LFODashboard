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
        public async Task<IActionResult> GetProfileById(string userId)
        {
            if (!Guid.TryParse(userId, out var guid))
                return BadRequest(ApiResponse<string>.FailResponse("Invalid userId", 400));

            var result = await _profileBL.GetProfileDetailsByIdAsync(guid);

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
        public async Task<IActionResult> InsertFleetOperatorbyType( UpdateFleetOperatorRequest request)
        {
            var result = await _profileBL.InsertFleetOperatorbyType(request);

            return Ok(ApiResponse<object>.SuccessResponse(result, "Profile saved successfully", 200));
        }
        #endregion
        #region Add Lane
        [HttpPost("lanes")]
        public async Task<IActionResult> InsertPreferredLane( PreferredLaneRequest request)
        {
            var result = await _profileBL.InsertPreferredLane(request);

            return Ok(ApiResponse<object>.SuccessResponse(result, "Lane Added  successfully", 200));
        }
        #endregion


        [HttpPost("upload")]
        public async Task<IActionResult> InsertFleetOperatorDocument(
            UpdateDocumentRequest request
           )
        {
            var result = await _profileBL.InsertFleetOperatorDocument(request);

            return Ok(ApiResponse<object>.SuccessResponse(result, "Documents uploaded successfully", 200));
        }
     

        #region Add Truck
        [HttpPost("trucks")]
        public async Task<IActionResult> InsertTruckDetails( TruckDetailsRequest request)
        {
            var result = await _profileBL.InsertTruckDetails(request);

            return Ok(ApiResponse<object>.SuccessResponse(result, "Truck Added  successfully", 200));
        }
        #endregion

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


        


    }
}