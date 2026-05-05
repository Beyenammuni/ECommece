using ClinicAppointment.Services.Interfaces;
using ClinicAppointmentHR.Data;
using ClinicAppointmentHR.Dtos.DoctorDto;
using ClinicAppointmentHR.Models;
using ClinicManagement.Main.Results;
using ClinicManagementSystem.App.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace ClinicAppointment.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _enviroment;
        public DoctorService(AppDbContext context, IWebHostEnvironment enviroment = null)
        {
            _context = context;
            _enviroment = enviroment;
        }

        public async Task<ServiceResult<List<DoctorModel>>> GetAllDoctorsAsync()
        {
            try
            {
                var doctors = await _context.Doctors
                    .Include(d => d.Department)
                    .Include(d => d.Appointments)
                    .ToListAsync();

                return ServiceResult<List<DoctorModel>>.Success(doctors,
                    $"Retrieved {doctors.Count} doctors successfully", 200);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<DoctorModel>>.Failure(
                    ex.Message, "Internal server error while retrieving doctors", 500);
            }
        }

        public async Task<ServiceResult<DoctorModel>> GetDoctorByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return ServiceResult<DoctorModel>.Failure(
                        "Invalid doctor ID. ID must be a positive integer","Invalid request",400);
                } 

                var doctor = await _context.Doctors
                    .Include(d => d.Department)
                    .Include(d => d.Appointments)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (doctor == null)
                {
                    return ServiceResult<DoctorModel>.Failure(
                        $"Doctor with ID {id} not found","Doctor not found", 404);
                }

                return ServiceResult<DoctorModel>.Success(
                    doctor,"Doctor retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                return ServiceResult<DoctorModel>.Failure(ex.Message,$"Internal server error: {ex.Message}",
                    500);
            }
        }

        public async Task<ServiceResult<DoctorModel>> CreateDoctorAsync(DoctorDto doctorDto)
        {
            try
            {
                if (doctorDto == null)
                {
                    return ServiceResult<DoctorModel>.Failure("Doctor data is required","Invalid request",
                        400);
                }
                if (string.IsNullOrWhiteSpace(doctorDto.Email) || !IsValidEmail(doctorDto.Email))
                {
                    return ServiceResult<DoctorModel>.Failure("Valid email address is required","Invalid email format",
                        400);
                }

                var existingDoctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.Email.ToLower() == doctorDto.Email.ToLower());

                if (existingDoctor != null)
                {
                    return ServiceResult<DoctorModel>.Failure(
                        "A doctor with this email already exists", "Duplicate email", 409);
                }
                var newDoctor = new DoctorModel
                {
                    FullName = doctorDto.FullName,
                    Specialty = doctorDto.Specialty, 
                    Email = doctorDto.Email,
                    DepartmentId = doctorDto.DepartmentId,
                };

                await _context.Doctors.AddAsync(newDoctor);
                await _context.SaveChangesAsync();

                await _context.Entry(newDoctor)
                    .Reference(d => d.Department).LoadAsync();

                return ServiceResult<DoctorModel>.Success(
                    newDoctor,"Doctor created successfully",201);
            }
            catch (Exception ex)
            {
                return ServiceResult<DoctorModel>.Failure(ex.Message,$"Internal server error: {ex.Message}",
                    500);
            }
        }

        public async Task<ServiceResult<DoctorModel>> UpdateDoctorAsync(int id, DoctorDto doctorDto)
        {
            try
            {

                if (id <= 0)
                {
                    return ServiceResult<DoctorModel>.Failure(
                        "Invalid doctor ID. ID must be a positive integer", "Invalid request", 400);
                }

                var existingDoctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (existingDoctor == null)
                {
                    return ServiceResult<DoctorModel>.Failure(
                        $"Doctor with ID {id} not found",
                        "Doctor not found", 404);
                }

                if (!string.IsNullOrWhiteSpace(doctorDto.Email) && !IsValidEmail(doctorDto.Email))
                {
                    return ServiceResult<DoctorModel>.Failure("Valid email address is required",
                        "Invalid email format",400 );
                }
                if (!string.IsNullOrWhiteSpace(doctorDto.Email) &&
                    doctorDto.Email != existingDoctor.Email)
                {
                    var emailExists = await _context.Doctors
                        .AnyAsync(d => d.Email.ToLower() == doctorDto.Email.ToLower() && d.Id != id);

                    if (emailExists)
                    {
                        return ServiceResult<DoctorModel>.Failure(
                            "Another doctor with this email already exists",
                            "Duplicate email",
                            409);
                    }
                }

                if (doctorDto.DepartmentId>0)
                {
                    var department = await _context.Departments
                        .FirstOrDefaultAsync(d => d.Id == doctorDto.DepartmentId);

                    if (department == null)
                    {
                        return ServiceResult<DoctorModel>.Failure(
                            $"Department with ID {doctorDto.DepartmentId} not found",
                            "Invalid department",
                            400);
                    }
                }
                if (!string.IsNullOrWhiteSpace(doctorDto.FullName))
                    existingDoctor.FullName = doctorDto.FullName;

                if (!string.IsNullOrWhiteSpace(doctorDto.Email))
                    existingDoctor.Email = doctorDto.Email;

                if (doctorDto.DepartmentId>0)
                    existingDoctor.DepartmentId = doctorDto.DepartmentId;

                if (doctorDto.Specialty != null)
                    existingDoctor.Specialty = doctorDto.Specialty;

                _context.Doctors.Update(existingDoctor);
                await _context.SaveChangesAsync();

                await _context.Entry(existingDoctor)
                    .Reference(d => d.Department)
                    .LoadAsync();

                return ServiceResult<DoctorModel>.Success(
                    existingDoctor, "Doctor updated successfully", 200);
            }
            catch (Exception ex)
            {
                return ServiceResult<DoctorModel>.Failure(
                    ex.Message, $"Internal server error: {ex.Message}", 500);
            }
        }

        public async Task<ServiceResult<bool>> DeleteDoctorAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return ServiceResult<bool>.Failure(
                        "Invalid doctor ID. ID must be a positive integer",
                        "Invalid request",
                        400);
                }

                var doctor = await _context.Doctors
                    .Include(d => d.Appointments)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (doctor == null)
                {
                    return ServiceResult<bool>.Failure(
                        $"Doctor with ID {id} not found",
                        "Doctor not found",
                        404);
                }

                if (doctor.Appointments != null && doctor.Appointments.Any(a => a.Status != StatusEnum.cancelled))
                {
                    return ServiceResult<bool>.Failure(
                        "Cannot delete doctor with active appointments. Please reassign or cancel appointments first.",
                        "Doctor has active appointments",
                        400);
                }

                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Success(true,
                    "Doctor deleted successfully", 204);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure(
                    ex.Message,
                    $"Internal server error: {ex.Message}",
                    500);
            }
        }

        public async Task<ServiceResult<List<DoctorModel>>> GetDoctorsByDepartmentAsync(int departmentId)
        {
            try
            {
                if (departmentId <= 0)
                {
                    return ServiceResult<List<DoctorModel>>.Failure("Invalid department ID. Department ID must be a positive integer",
                        "Invalid request", 400);
                }

                var department = await _context.Departments
                    .FirstOrDefaultAsync(d => d.Id == departmentId);

                if (department == null)
                {
                    return ServiceResult<List<DoctorModel>>.Failure(
                        $"Department with ID {departmentId} not found", "Department not found", 404);
                }

                var doctors = await _context.Doctors
                    .Where(d => d.DepartmentId == departmentId)
                    .Include(d => d.Department)
                    .ToListAsync();

                return ServiceResult<List<DoctorModel>>.Success(
                    doctors,
                    doctors.Any() ? $"Found {doctors.Count} doctors in department" : "No doctors found in this department",
                    200);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<DoctorModel>>.Failure(
                    ex.Message, $"Internal server error: {ex.Message}",500);
            }
        }

        public async Task<ServiceResult<List<DoctorModel>>> GetDoctorsBySpecialtyAsync(SpecialityEnum specialty)
        {
            try
            {
                var doctors = await _context.Doctors
                    .Where(d => d.Specialty == specialty)
                    .Include(d => d.Department)
                    .ToListAsync();

                return ServiceResult<List<DoctorModel>>.Success(
                    doctors,
                    doctors.Any() ? $"Found {doctors.Count} doctors with specialty {specialty}" : $"No doctors found with specialty {specialty}",
                    200);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<DoctorModel>>.Failure(
                    ex.Message,$"Internal server error: {ex.Message}",500);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public async Task<ServiceResult<string>> UploadProfileImageAsync(int doctorId, IFormFile file)
        {
            try
            {
                var doctor = await _context.Doctors.FindAsync(doctorId);
                if (doctor == null)
                    return ServiceResult<string>.Failure("Doctor not found", "Not found", 404);

                if (file == null || file.Length == 0)
                    return ServiceResult<string>.Failure("No file provided", "Error", 400);

                var allowedTypes = new[] { "jpeg", "png", "jpg" };
                if (!allowedTypes.Contains(file.ContentType))
                    return ServiceResult<string>.Failure("Only JPEG and PNG images are allowed", "Error", 400);

                if (file.Length > 2 * 1024 * 1024)
                    return ServiceResult<string>.Failure("File size must be less than 2MB", "Error", 400);

                var uploadsDir = Path.Combine(_enviroment.WebRootPath, "uploads", "doctors");
                if (!Directory.Exists(uploadsDir))
                    Directory.CreateDirectory(uploadsDir);

                var fileName = $"{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadsDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var imageUrl = $"/uploads/doctors/{fileName}";

                if (!string.IsNullOrEmpty(doctor.ProfileImageUrl))
                {
                    var oldPath = Path.Combine(_enviroment.WebRootPath, doctor.ProfileImageUrl.TrimStart('/'));
                    if (File.Exists(oldPath))
                        File.Delete(oldPath);
                }

                doctor.ProfileImageUrl = imageUrl;
                await _context.SaveChangesAsync();

                return ServiceResult<string>.Success(imageUrl, "Profile image uploaded successfully", 200);
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Failure(ex.Message, "Error", 500);
            }
        }

        public async Task<ServiceResult<string>> GetProfileImageUrlAsync(int doctorId)
        {
            try
            {
                var doctor = await _context.Doctors.FindAsync(doctorId);
                if (doctor == null)
                    return ServiceResult<string>.Failure("Doctor not found", "Not found", 404);

                return ServiceResult<string>.Success(doctor.ProfileImageUrl, "Image URL retrieved", 200);
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Failure(ex.Message, "Error", 500);
            }
        }

        public async Task<ServiceResult<bool>> DeleteProfileImageAsync(int doctorId)
        {
            try
            {
                var doctor = await _context.Doctors.FindAsync(doctorId);
                if (doctor == null)
                    return ServiceResult<bool>.Failure("Doctor not found", "Not found", 404);

                if (!string.IsNullOrEmpty(doctor.ProfileImageUrl))
                {
                    var filePath = Path.Combine(_enviroment.WebRootPath, doctor.ProfileImageUrl.TrimStart('/'));
                    if (File.Exists(filePath))
                        File.Delete(filePath);

                    doctor.ProfileImageUrl = null;
                    await _context.SaveChangesAsync();
                }

                return ServiceResult<bool>.Success(true, "Profile image deleted", 200);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure(ex.Message, "Error", 500);
            }
        }
    }
}