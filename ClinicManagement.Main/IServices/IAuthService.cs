using ClinicAppointmentHR.Helper;
using ClinicManagement.App.Dtos.RegistrationDtos;
using ClinicManagement.App.Dtos.RegistrationDtos.Request;
using ClinicManagement.App.Dtos.RegistrationDtos.Response;
using ClinicManagement.App.Models;
using ClinicManagement.Main.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicManagement.Main.IServices
{
    public interface IAuthService
    {
            Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginRequest loginDto);
            Task<ServiceResult<AuthResponseDto>> RegisterAsync(UserModel registerDto);
            Task<ServiceResult<bool>> AssignRoleAsync(string userId, string role);
            Task<ServiceResult<bool>> ChangePasswordAsync(string userId, string currentPassword, string newPassword);

            Task<ServiceResult<bool>> LogoutAsync(string userId);
            Task<ServiceResult<UserInfoDto>> GetCurrentUserInfoAsync(string userId);
        }
    }
