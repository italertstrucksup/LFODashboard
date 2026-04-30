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
        public async Task<IActionResult> SendSignupOtp(SignupOtpRequest request)
        {
            var result = await _authBL.SendSignupOtp(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        // POST api/auth/signup_user
        [HttpPost("signup_user")]
        public async Task<IActionResult> SignupAsync( SignupRequest request)
        {
            var result = await _authBL.UserRegister(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // POST api/auth/verify_Otp
        [HttpPost("verifyOtp")]
        public async Task<IActionResult> VerifyOTP( OTPVerifyRequest request)
        {
            var result = await _authBL.VerifySignupOTP(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login( LoginReq request)
        {
            var result = await _authBL.UserLogin(request);
            return result.Success ? Ok(result) : Unauthorized(result);
        }


        // POST api/auth/refresh-token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken( LoginReq request)
        {
            var result = await _authBL.RefreshToken(request);
            return result.Success ? Ok(result) : Unauthorized(result);
        }

        // POST api/auth/send-login-otp
        [HttpPost("send-login-otp")]
        public async Task<IActionResult> SendLoginOtp( LoginOtpRequest request)
        {
            var result = await _authBL.SendLoginOtp(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // POST api/auth/login-with-otp
        [HttpPost("login-with-otp")]
        public async Task<IActionResult> LoginWithOtp( LoginOtpRequest request)
        {
            var result = await _authBL.LoginWithOtp(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // POST api/auth/send-reset-otp
        [HttpPost("send-reset-otp")]
        public async Task<IActionResult> SendResetOtp(ResetPasswordOTPRequest request)
        {
            var result = await _authBL.SendResetOtp(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // POST api/auth/verify-reset-otp
        [HttpPost("verify-reset-otp")]
        public async Task<IActionResult> VerifyOTPResetPassword(VerifyOTPReq request)
        {
            var result = await _authBL.VerifyOTPResetPassword(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // POST api/auth/reset-password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordReq request)
        {
            var result = await _authBL.ResetPassword(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        //-------------------LOGOUT-------------------

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var refreshToken = Request.Headers["RefreshToken"].FirstOrDefault();

                if (string.IsNullOrWhiteSpace(userIdStr) || string.IsNullOrWhiteSpace(refreshToken))
                {
                    return Ok(new { Success = true, Message = "Logged out successfully" });
                }

                if (!Guid.TryParse(userIdStr, out Guid userId))
                {
                    return Ok(new { Success = true, Message = "Logged out successfully" });
                }

                var result = await _authBL.RevokeToken(userId, refreshToken);

                return Ok(result);
            }
            catch
            {
                return Ok(new { Success = true, Message = "Logged out successfully" });
            }
        }


    }
}
