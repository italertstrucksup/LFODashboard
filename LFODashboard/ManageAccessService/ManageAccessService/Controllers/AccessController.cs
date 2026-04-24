using ManageAccessService.BL.Interface;
using ManageAccessService.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManageAccessService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly IAccessBL _accessBL;

        public AccessController(IAccessBL accessBL)
        {
            _accessBL = accessBL;
        }

        [HttpPost("add_subuser")]
        public async Task<IActionResult> AddUser([FromBody] UserApiRequest user)
        {
             return Ok(await _accessBL.AddUserAsync(user));
            
        }

        [HttpPost("edit_subuser")]
        public async Task<IActionResult> EditUser([FromBody] UserApiRequest user)
        {
             return Ok(await _accessBL.EditUserAsync(user));
            
        }

        [HttpPost("get_subuser")]
        public async Task<IActionResult> GetUser([FromBody] GetVehicleRequest request)
        {
             return Ok(await _accessBL.GetUserVehicleDataAsync(request));
            
        }

        [HttpPost("get_vehicle")]
        public async Task<IActionResult> GetVehicle([FromBody] GetVehicleRequest request)
        {
             return Ok(await _accessBL.GetVehicleListAsync(request));
            
        }

    }
}
