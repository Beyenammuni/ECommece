using ClinicAppointmentHR.Data;
using ClinicAppointmentHR.Helper;
using ClinicAppointmentHR.IServices;
using ClinicManagement.App.Dtos.RegistrationDtos;
using ClinicManagement.App.Dtos.RegistrationDtos.Request;
using ClinicManagement.App.Dtos.RegistrationDtos.Response;
using ClinicManagement.App.Models;
using ClinicManagement.Main.IServices;
using ClinicManagement.Main.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClinicManagement.Main.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AuthService(
            UserManager<UserModel> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<ServiceResult<bool>> AssignRoleAsync(string userId, string role)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult<bool>.Failure("User not found", "Not found", 404);
                }

                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, role);

                return ServiceResult<bool>.Success(true, $"Role {role} assigned successfully", 200);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure(
                    ex.Message,
                    "An error occurred while assigning role",
                    500);
            }
        }

        public async Task<ServiceResult<bool>> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId); 
                if (user == null)
                {
                    return ServiceResult<bool>.Failure("User not found", "Not found", 404);
                }

                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return ServiceResult<bool>.Failure($"Password change failed: {errors}", "Error", 400);
                }
                return ServiceResult<bool>.Success(true, "Password changed successfully", 200);
            }
            catch (Exception e)
            {
                return ServiceResult<bool>.Failure("An error occurred while changing password", e.Message, 500);
            }
        }

        public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginRequest loginDto)
        {
            try
            {               
                var user = await _userManager.FindByNameAsync(loginDto.Username);

                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(loginDto.Username);
                }

                if (user == null)
                {
                    return ServiceResult<AuthResponseDto>.Failure(
                        "Invalid username or password",
                        "Authentication failed",
                        401);
                }

                var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!isPasswordValid)
                {
                    return ServiceResult<AuthResponseDto>.Failure(
                        "Invalid username or password",
                        "Authentication failed",
                        401);
                }

                if (!user.IsActive)
                {
                    return ServiceResult<AuthResponseDto>.Failure(
                        "Account is deactivated. Please contact administrator.",
                        "Authentication failed",
                        401);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var userRole = roles.FirstOrDefault() ?? "User";

                var token = await GenerateJwtTokenAsync(user, userRole);

                return ServiceResult<AuthResponseDto>.Success(
                    new AuthResponseDto
                    {
                        Token = token,
                        Username = user.UserName,
                        Email = user.Email,
                        Role = userRole,
                        ExpiresAt = DateTime.UtcNow.AddHours(1)
                    },
                    "Login Successfully",
                    200);
            }
            catch (Exception ex)
            {
                return ServiceResult<AuthResponseDto>.Failure(
                    "An error occurred during login",
                    ex.Message,
                    500);
            }
        }

        public async Task<ServiceResult<AuthResponseDto>> RegisterAsync(UserModel registerDto)
        {
            try
            {
                var existingUser = await _userManager.FindByNameAsync(registerDto.Username);
                if (existingUser != null)
                {
                    return ServiceResult<AuthResponseDto>.Failure(
                        "Username already exists",
                        "Registration failed",
                        400);
                }

                var existingEmail = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingEmail != null)
                {
                    return ServiceResult<AuthResponseDto>.Failure(
                        "Email already exists",
                        "Registration failed",
                        400);
                }

                var user = new UserModel
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                    FullName = registerDto.FullName,
                    CreateAt = DateTime.UtcNow,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return ServiceResult<AuthResponseDto>.Failure(
                        $"User registration failed: {errors}",
                        "Registration failed",
                        400);
                }

                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }

                await _userManager.AddToRoleAsync(user, "User");

                var token = await GenerateJwtTokenAsync(user, "User");

                return ServiceResult<AuthResponseDto>.Success(
                    new AuthResponseDto
                    {
                        Token = token,
                        Username = user.UserName,
                        Email = user.Email,
                        Role = "User",
                        ExpiresAt = DateTime.UtcNow.AddHours(1)
                    },
                    "Registration Successfully",
                    201);
            }
            catch (Exception ex)
            {
                return ServiceResult<AuthResponseDto>.Failure(
                    "An error occurred during registration",
                    ex.Message,
                    500);
            }
        }

        public async Task<ServiceResult<bool>> LogoutAsync(string userId)
        {
            try
            {
                return ServiceResult<bool>.Success(true, "Logged out successfully", 200);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure(ex.Message, "Error during logout", 500);
            }
        }

        public async Task<ServiceResult<UserInfoDto>> GetCurrentUserInfoAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult<UserInfoDto>.Failure("User not found", "Not found", 404);
                }

                var roles = await _userManager.GetRolesAsync(user);

                var userInfo = new UserInfoDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName,
                    Roles = roles.ToList(),
                    IsActive = user.IsActive
                };

                return ServiceResult<UserInfoDto>.Success(userInfo, "User info retrieved", 200);
            }
            catch (Exception ex)
            {
                return ServiceResult<UserInfoDto>.Failure(ex.Message, "Error", 500);
            }
        }

        private async Task<string> GenerateJwtTokenAsync(UserModel user, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FullName ?? user.UserName),
                new Claim(ClaimTypes.Role, role ?? "User"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}