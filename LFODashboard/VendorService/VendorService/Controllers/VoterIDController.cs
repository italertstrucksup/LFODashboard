using Common.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VendorService.BL.Implementation;
using VendorService.BL.Interface;
using VendorService.Models;


namespace VendorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoterIDController : ControllerBase
    {
        private readonly IVoterService _voterService;
        public VoterIDController(IVoterService voterService)
        {
            _voterService = voterService;
        }

        [HttpPost]
        [Route("verify-voterid")]
        public async Task<IActionResult> VerifyVoterAsync(ApiRequest<VIDVerificationRequest> request)
        {
            var result = await _voterService.VerifyVoterAsync(request.Data);

            return Ok(result);
        }

    }
}
