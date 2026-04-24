using Common.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VendorService.BL.Interface;
using VendorService.Models;

namespace VendorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RCDetailController : ControllerBase
    {
        private readonly IRCDetailsService _rCDetailsService;
        public RCDetailController(IRCDetailsService rCDetailsService)
        {
            _rCDetailsService= rCDetailsService;
        }

        [HttpPost]
        [Route("rc-verify")]
        public async Task<IActionResult> SendRCVerifyAsync(ApiRequest<RCDetailsAPIRequest> request)
        {
            var result = await _rCDetailsService.SendRCVerifyAsync(request.Data);

            return Ok(result);
        }
    }
}
