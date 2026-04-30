using DashboardService.BL.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DashboardService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardBL _dashboardBL;

        public DashboardController(IDashboardBL dashboardBL)
        {
            _dashboardBL = dashboardBL;
        }

        [HttpGet("GetDashboardData")]
        public async Task<IActionResult> GetDashboardData()
        {
            var response = await _dashboardBL.GetDashboardDataAsync();

            if (response.Success)
            {
                return Ok(response);
            }

            return StatusCode(response.StatusCode, response);
        }
    }
}
