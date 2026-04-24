using MediaService.BL.IBussinessLayer;
using Microsoft.AspNetCore.Mvc;
using Common.Core;
using MediaService.Model.Model;

namespace MediaServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> UploadBase64([FromBody] ApiRequest<MediaUploadRequest> request)
        {
            if (request?.Data == null || string.IsNullOrEmpty(request.Data.File))
            {
                return BadRequest(ApiResponse<object>.FailResponse("Invalid request data", 400));
            }

            try
            {
                // Convert Base64 back to a file/stream 
                byte[] bytes = Convert.FromBase64String(request.Data.File);
                using (var stream = new MemoryStream(bytes))
                {
                    var file = new FormFile(stream, 0, bytes.Length, "file", "image.jpg");
                    var response = await _mediaBusinessLayer.UploadDocumentAsync(file, request.Data.FolderName);
                    return StatusCode(response.StatusCode, response);
                }
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
