using ClinicAppointmentHR.Data;
using ClinicAppointmentHR.Dtos.AppointmentDtos;
using ClinicAppointmentHR.Models;
using ClinicManagement.App.Dtos.BonusDtos;
using ClinicManagement.Main.Results;
using ClinicManagementSystem.App.Dtos.AppointmentDtos;
using ClinicManagementSystem.App.Enums;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.App.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<ServiceResult<AppointmentModel>> ScheduleAppointmentAsync(ScheduletAppointmentDto schedule);
        Task<ServiceResult<AppointmentModel>> UpdateAppointmentAsync(int id, UpdateAppoitmentDto update);
        Task<ServiceResult<bool>> CancelAppointmentAsync(int id);
        Task<ServiceResult<List<AppointmentModel>>> GetCompletedAppointmentsAsync();
        Task<ServiceResult<List<AppointmentModel>>> GetAppointmentsByDateAndDoctorAsync(getAppointmentByDotorAndDateDto appointment);
        Task<ServiceResult<List<AppointmentModel>>> GetTodayAppointmentsAsync();
        Task<ServiceResult<List<AppointmentModel>>> GetAllAppointmentsAsync();
        Task<ServiceResult<AppointmentModel>> GetAppointmentByIdAsync(int id);
        Task<ServiceResult<AppointmentModel>> CreateAppointmentAsync(AppointmentDto appointment);
        Task<ServiceResult<bool>> DeleteAppointmentAsync(int id);
        Task<ServiceResult<object>> GetFilteredAppointmentsAsync(AppointmentFilterDto filter);

    }
}