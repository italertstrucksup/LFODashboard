using Microsoft.AspNetCore.Mvc;
using ProfileService_LFO.BL.Interface;
using Common.Core;

namespace ProfileService_LFO.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GetProfileController : ControllerBase
    {
        private readonly IprofileDetails_BL _profileBL;

        public GetProfileController(IprofileDetails_BL profileBL)
        {
            _profileBL = profileBL;
        }

        #region Get Profile By Id
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfileById(int userId)
        {
            var result = await _profileBL.GetProfileDetailsByIdAsync(userId);

            if (result == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Profile not found", 404));
            }

            return Ok(ApiResponse<object>.SuccessResponse(result, "Profile fetched successfully", 200));
        }
        #endregion

        #region Get Lanes
        [HttpGet("lanes/{loginId}")]
        public async Task<IActionResult> GetLanes(long loginId)
        {
            var result = await _profileBL.GetLanesAsync(loginId);

            return Ok(ApiResponse<object>.SuccessResponse(result, "Lanes fetched successfully", 200));
        }
        #endregion

        #region Get KYC
        [HttpGet("kyc/{profileId}")]
        public async Task<IActionResult> GetKYC(long profileId)
        {
            var result = await _profileBL.GetKYCAsync(profileId);

            return Ok(ApiResponse<object>.SuccessResponse(result, "KYC details fetched successfully", 200));
        }
        #endregion

        #region Get Trucks
        [HttpGet("trucks/{profileId}")]
        public async Task<IActionResult> GetTrucks(long profileId)
        {
            var result = await _profileBL.GetTrucksAsync(profileId);

            return Ok(ApiResponse<object>.SuccessResponse(result, "Truck details fetched successfully", 200));
        }
        #endregion

        #region Get All Registration Data
        [HttpGet("all-data/{userId}")]
        public async Task<IActionResult> GetAllRegistrationData(int userId)
        {
            var result = await _profileBL.GetCompleteRegistrationDataAsync(userId);

            return Ok(ApiResponse<object>.SuccessResponse(result, "Complete registration data fetched successfully", 200));
        }
        #endregion
    }
}
