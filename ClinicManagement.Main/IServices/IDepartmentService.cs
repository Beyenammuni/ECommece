using ClinicAppointmentHR.Dtos.DepartmentDto;
using ClinicAppointmentHR.Models;
using ClinicManagement.Main.Results;
using ClinicManagementSystem.App.Dtos.DepartmentDtos;

namespace ClinicAppointment.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<ServiceResult<List<DepartmentModel>>> GetAllDepartmentsAsync();
        Task<ServiceResult<DepartmentModel>> GetDepartmentByIdAsync(int id);
        Task<ServiceResult<DepartmentModel>> CreateDepartmentAsync(DepartmentDto departmentDto);
        Task<ServiceResult<DepartmentModel>> UpdateDepartmentAsync(int id, DepartmentDto departmentDto);
        Task<ServiceResult<bool>> DeleteDepartmentAsync(int id);
        Task<ServiceResult<List<DoctorModel>>> GetDoctorsByDepartmentIdAsync(int departmentId);
    }
}