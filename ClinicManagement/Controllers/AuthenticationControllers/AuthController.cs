using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ClinicAppointmentHR.Data;
using ClinicAppointmentHR.Helper;
using ClinicManagement.Main.IServices;
using Microsoft.AspNetCore.Identity.Data;
using ClinicManagement.App.Dtos.RegistrationDtos.Request;
using LoginRequest = ClinicAppointmentHR.Helper.LoginRequest;
using ClinicManagement.App.Models;

namespace ClinicAppointment.Controllers.LogginControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(JwtSettings jwtSettings, IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _authService.RegisterAsync(request);
                return StatusCode(result.StatusCode, new
                {
                    result.IsSuccess,
                    result.Message,
                    result.Data,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _authService.LoginAsync(request);
                return StatusCode(result.StatusCode, new
                {
                    result.IsSuccess,
                    result.Message,
                    result.Data,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string userId, string currentPassword, string newPassword)
        {
            try
            {
                var result = await _authService.ChangePasswordAsync(userId, currentPassword, newPassword);
                return StatusCode(result.StatusCode, new
                {
                    result.IsSuccess,
                    result.Message,
                    result.Data,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /*[HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "admin" && request.Password == "1234")
            {
                var token = GenerateJwtToken(request.Username);
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }

        private string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }*/
    }
}
