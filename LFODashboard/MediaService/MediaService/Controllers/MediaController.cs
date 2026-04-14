using MediaService.BL.IBussinessLayer;
using MediaService.Model.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


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
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, Message = ex.Message });
            }
        }

        //[HttpDelete("delete/{*documentKey}")]
        //public async Task<IActionResult> DeleteDocument([FromRoute] string documentKey)
        //{
        //    try
        //    {
        //        string decodedKey = System.Net.WebUtility.UrlDecode(documentKey);
        //        var result = await _mediaBusinessLayer.DeleteDocumentAsync(decodedKey);
        //        return Ok(new { success = true, deleted = result });
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}
    }
}
