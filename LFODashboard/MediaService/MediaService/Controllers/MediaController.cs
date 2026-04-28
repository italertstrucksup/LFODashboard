using MediaService.BL.IBussinessLayer;
using Microsoft.AspNetCore.Mvc;
using Common.Core;
using MediaService.Model.Model;

namespace MediaServiceAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly IMediaBusinessLayer _mediaBusinessLayer;

        public MediaController(IMediaBusinessLayer mediaBusinessLayer)
        {
            _mediaBusinessLayer = mediaBusinessLayer;
        }

        [HttpPost("upload", Name = "UploadDocument")]
        public async Task<IActionResult> UploadDocument(IFormFile file, [FromQuery] string folderName)
        {
            var response = await _mediaBusinessLayer.UploadDocumentAsync(file, folderName);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("upload-base64", Name = "UploadBase64")]
        public async Task<IActionResult> UploadBase64(ApiRequest<MediaUploadRequest> request)
        {
            if (request?.Data == null || string.IsNullOrEmpty(request.Data.File))
            {
                return BadRequest(ApiResponse<object>.FailResponse("Invalid request data", 400));
            }

            try
            {
                var response = await _mediaBusinessLayer.UploadBase64Async(request.Data.File, request.Data.FolderName);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.FailResponse(ex.Message, 500));
            }
        }

        [HttpGet("get/{*documentKey}")]
        public IActionResult GetDocument([FromRoute] string documentKey)
        {
            try
            {
                string decodedKey = System.Net.WebUtility.UrlDecode(documentKey);
                var response = _mediaBusinessLayer.GetPreSignedUrl(decodedKey);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.FailResponse(ex.Message, 500));
            }
        }
    }
}
