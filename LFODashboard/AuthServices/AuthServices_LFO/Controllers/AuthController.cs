using AuthServices_LFO.BL.Interface;
using AuthServices_LFO.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthServices_LFO.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    
    public class AuthController : ControllerBase
    {
        private readonly IAuthBL _authBL;

        public AuthController(IAuthBL authBL)
        {
            _authBL = authBL;
        }

        //------------------------SIGNUP-------------------------
        // POST api/auth/send_signup_otp
        [HttpPost("send_signup_otp")]
        public async Task<IActionResult> SendSignupOtp([FromBody] SignupRequest request)
        {
            var result = await _authBL.SendSignupOtpAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        // POST api/auth/signup_user
        [HttpPost("signup_user")]
        public async Task<IActionResult> SignupAsync([FromBody] SignupRequest request)
        {
            var result = await _authBL.SignupAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // POST api/auth/verify_Otp
        [HttpPost("verifyOtp")]
        public async Task<IActionResult> VerifyOTP([FromBody] OTPVerifyRequest request)
        {
            var result = await _authBL.VerifyOTPAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authBL.LoginAsync(request);
            return result.Success ? Ok(result) : Unauthorized(result);
        }


        // POST api/auth/refresh-token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] LoginRequest request)
        {
            var result = await _authBL.RefreshTokenAsync(request);
            return result.Success ? Ok(result) : Unauthorized(result);
        }

        [HttpPost("send-login-otp")]
        public async Task<IActionResult> SendLoginOtp([FromBody] LoginOtpRequest request)
        {
            var result = await _authBL.SendLoginOtpAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login-with-otp")]
        public async Task<IActionResult> LoginWithOtp([FromBody] LoginOtpRequest request)
        {
            var result = await _authBL.LoginWithOtpAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // POST api/auth/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userIdStr = User.Claims.FirstOrDefault(c =>
            c.Type == ClaimTypes.NameIdentifier ||
            c.Type == "nameid" ||
            c.Type.EndsWith("nameidentifier"))?.Value;

            if (!int.TryParse(userIdStr, out int userId))
                return BadRequest(new { Message = "Invalid token" });

            var result = await _authBL.RevokeTokenAsync(userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // GET api/auth/me
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetMe()
        {
            return Ok(new
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Name = User.FindFirst(ClaimTypes.Name)?.Value,
                Role = User.FindFirst(ClaimTypes.Role)?.Value,
                EntityType = User.FindFirst("EntityType")?.Value
            });
        }
    }
}
