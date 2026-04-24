using Common.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VendorService.BL.Interface;
using VendorService.Models;

namespace VendorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PanController : ControllerBase
    {
        private readonly IPanService _panService;
        public PanController(IPanService panService)
        {
            _panService = panService;
        }


        [HttpPost]
        [Route("verify-pan")]
        public async Task<IActionResult> VerifyPanAsync(ApiRequest<PanVerifyRequest> request)
        {
            var result = await _panService.VerifyPanAsync(request.Data);

            return Ok(result);
        }

    }
}
