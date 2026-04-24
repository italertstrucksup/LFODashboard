using Microsoft.AspNetCore.Mvc;
using ProfilePhotoService.BL.Interface;
using ProfilePhotoService.Model.Models;

namespace ProfilePhotoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilePhotoController : ControllerBase
    {
        private readonly IProfilePhotoBL _profilePhotoBL;

        public ProfilePhotoController(IProfilePhotoBL profilePhotoBL)
        {
            _profilePhotoBL = profilePhotoBL;
        }

        [HttpPost("send-upload-link")]
        public async Task<IActionResult> SendUploadLink([FromBody] UploadLinkRequest request)
        {
            var result = await _profilePhotoBL.SendUploadLinkAsync(request);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("resend-upload-link")]
        public async Task<IActionResult> ResendUploadLink([FromBody] UploadLinkRequest request)
        {
            var result = await _profilePhotoBL.ResendUploadLinkAsync(request);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPhoto([FromBody] PhotoUploadRequest request)
        {
            var result = await _profilePhotoBL.UploadPhotoAsync(request);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("status/{loginId}")]
        public async Task<IActionResult> GetStatus(Guid loginId)
        {
            var result = await _profilePhotoBL.GetUploadStatusAsync(loginId);
            if (result.Success) return Ok(result);
            return NotFound(result);
        }
    }
}
