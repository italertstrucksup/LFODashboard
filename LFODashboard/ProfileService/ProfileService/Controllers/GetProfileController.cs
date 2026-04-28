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


        [HttpGet("all-data/{userId}")]
        public async Task<IActionResult> GetAllRegistrationData(Guid userId)
        {
            var result = await _profileBL.GetCompleteKYCDataAsync(userId);

            return StatusCode(result.StatusCode, result);
        }
    }
}
