using ClinicAppointment.Services.Interfaces;
using ClinicAppointmentHR.Data;
using ClinicAppointmentHR.Dtos.DepartmentDto;
using ClinicAppointmentHR.Models;
using ClinicManagement.Main.Results;
using ClinicManagementSystem.App.Dtos.DepartmentDtos;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointment.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly AppDbContext _context;

        public DepartmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<DepartmentModel>>> GetAllDepartmentsAsync()
        {
            try
            {
                var departments = await _context.Departments.ToListAsync();
                return ServiceResult<List<DepartmentModel>>.Success(departments,
                    "Departments retrieved successfully",200);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<DepartmentModel>>.Failure(
                    ex.Message, "Internal server error",500);
            }
        }

        public async Task<ServiceResult<DepartmentModel>> GetDepartmentByIdAsync(int id)
        {
            try
            {
                var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);

                if (department == null)
                {
                    return ServiceResult<DepartmentModel>.Failure(
                        $"Department with ID {id} not found","Department not found",404);
                }

                return ServiceResult<DepartmentModel>.Success(
                    department,"Department retrieved successfully",200);
            }
            catch (Exception ex)
            {
                return ServiceResult<DepartmentModel>.Failure(
                    ex.Message,"Internal server error",500);
            }
        }

        public async Task<ServiceResult<DepartmentModel>> CreateDepartmentAsync(DepartmentDto departmentDto)
        {
            try
            {
                if (departmentDto == null)
                {
                    return ServiceResult<DepartmentModel>.Failure("Department data is required",
                        "Invalid request",400);
                }

                var existingDepartment = await _context.Departments
                    .FirstOrDefaultAsync(d => d.Name.ToLower() == departmentDto.Name.ToLower());

                if (existingDepartment != null)
                {
                    return ServiceResult<DepartmentModel>.Failure(
                        "Department with this name already exists",
                        "Duplicate department",409);
                }

                var newDepartment = new DepartmentModel
                {
                    Name = departmentDto.Name,
                    Floor = departmentDto.Floor,
                    Description = departmentDto.Description
                };

                await _context.Departments.AddAsync(newDepartment);
                await _context.SaveChangesAsync();

                return ServiceResult<DepartmentModel>.Success(
                    newDepartment,"Department created successfully", 201);
            }
            catch (Exception ex)
            {
                return ServiceResult<DepartmentModel>.Failure(
                    ex.Message,$"Internal server error: {ex.Message}",
                    500);
            }
        }

        public async Task<ServiceResult<DepartmentModel>> UpdateDepartmentAsync(int id, DepartmentDto departmentDto)
        {
            try
            {
                if (departmentDto == null)
                {
                    return ServiceResult<DepartmentModel>.Failure(
                        "Department data is required",
                        "Invalid request",
                        400);
                }

                var existingDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);

                if (existingDepartment == null)
                {
                    return ServiceResult<DepartmentModel>.Failure(
                        $"Department with ID {id} not found","Department not found",404);
                }
                var nameConflict = await _context.Departments
                    .AnyAsync(d => d.Name.ToLower() == departmentDto.Name.ToLower() && d.Id != id);

                if (nameConflict)
                {
                    return ServiceResult<DepartmentModel>.Failure(
                        "Another department with this name already exists",
                        "Duplicate department name",409);
                }

                existingDepartment.Name = departmentDto.Name;
                existingDepartment.Floor = departmentDto.Floor;
                existingDepartment.Description = departmentDto.Description;

                _context.Departments.Update(existingDepartment);
                await _context.SaveChangesAsync();

                return ServiceResult<DepartmentModel>.Success(
                    existingDepartment, "Department updated successfully",200);
            }
            catch (Exception ex)
            {
                return ServiceResult<DepartmentModel>.Failure(
                    ex.Message, $"Internal server error: {ex.Message}", 500);
            }
        }

        public async Task<ServiceResult<bool>> DeleteDepartmentAsync(int id)
        {
            try
            {
                var department = await _context.Departments
                    .Include(d => d.Doctors)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (department == null)
                {
                    return ServiceResult<bool>.Failure(
                        $"Department with ID {id} not found", "Department not found", 404);
                }

                if (department.Doctors != null && department.Doctors.Any())
                {
                    return ServiceResult<bool>.Failure(
                        "Cannot delete department with associated doctors",
                        "Department has doctors assigned",
                        400);
                }

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Success(true  ,
                    "Department deleted successfully",204);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure(
                    ex.Message,$"Internal server error: {ex.Message}", 500);
            }
        }

        public async Task<ServiceResult<List<DoctorModel>>> GetDoctorsByDepartmentIdAsync(int departmentId)
        {
            try
            {
                if (departmentId <= 0)
                {
                    return ServiceResult<List<DoctorModel>>.Failure(
                        "Invalid department ID. Department ID must be a positive integer.",
                        "Invalid request", 400);
                }

                var department = await _context.Departments
                    .Include(d => d.Doctors)
                    .FirstOrDefaultAsync(d => d.Id == departmentId);

                if (department == null)
                {
                    return ServiceResult<List<DoctorModel>>.Failure(
                        $"Department with ID {departmentId} not found", "Department not found", 404);
                }

                var doctors = department.Doctors?.ToList() ?? new List<DoctorModel>();

                return ServiceResult<List<DoctorModel>>.Success(
                    doctors,
                    doctors.Any()
                        ? $"Found {doctors.Count} doctors in department"
                        : "No doctors found in this department",200);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<DoctorModel>>.Failure(ex.Message,
                    $"Internal server error: {ex.Message}", 500);
            }
        }
    }
}