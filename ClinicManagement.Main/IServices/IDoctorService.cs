using ClinicAppointmentHR.Dtos.DoctorDto;
using ClinicAppointmentHR.Models;
using ClinicManagement.Main.Results;
using ClinicManagementSystem.App.Enums;
using Microsoft.AspNetCore.Http;

namespace ClinicAppointment.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<ServiceResult<List<DoctorModel>>> GetAllDoctorsAsync();
        Task<ServiceResult<DoctorModel>> GetDoctorByIdAsync(int id);
        Task<ServiceResult<DoctorModel>> CreateDoctorAsync(DoctorDto doctorDto);
        Task<ServiceResult<DoctorModel>> UpdateDoctorAsync(int id, DoctorDto doctorDto);
        Task<ServiceResult<bool>> DeleteDoctorAsync(int id);
        Task<ServiceResult<List<DoctorModel>>> GetDoctorsByDepartmentAsync(int departmentId);
        Task<ServiceResult<List<DoctorModel>>> GetDoctorsBySpecialtyAsync(SpecialityEnum specialty);
        Task<ServiceResult<string>> UploadProfileImageAsync(int doctorId, IFormFile file);
        Task<ServiceResult<string>> GetProfileImageUrlAsync(int doctorId);
        Task<ServiceResult<bool>> DeleteProfileImageAsync(int doctorId);
    }
}