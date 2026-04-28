using ManageAccessService.BL.Interface;
using ManageAccessService.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManageAccessService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionBL _permissionBL;

        public PermissionController(IPermissionBL permissionBL)
        {
            _permissionBL = permissionBL;
        }

        [HttpPost("get_modules")]
        public async Task<IActionResult> GetModules([FromBody] PermissionRequest req)
        {
            return Ok(await _permissionBL.GetModules(req));

        }

        [HttpPost("get_features")]
        public async Task<IActionResult> GetFeatures([FromBody] PermissionRequest req)
        {
            return Ok(await _permissionBL.GetFeatures(req));

        }
    }
}
