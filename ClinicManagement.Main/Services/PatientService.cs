using ClinicAppointment.Services.Interfaces;
using ClinicAppointmentHR.Data;
using ClinicAppointmentHR.Dtos.PatientDtos;
using ClinicAppointmentHR.Models;
using ClinicManagement.Main.Results;
using ClinicManagementSystem.App.Dtos.PatientDtos;
using ClinicManagementSystem.App.Enums;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace ClinicAppointment.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly AppDbContext _context;

        public PatientService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<PatientModel>>> GetAllPatientsAsync()
        {
            try
            {
                var patients = await _context.Patiens
                    .Include(p => p.Appointments)
                    .OrderBy(p => p.FullName)
                    .ToListAsync();

                return ServiceResult<List<PatientModel>>.Success(
                    patients,
                    patients.Any() ? $"Retrieved {patients.Count} patients successfully" : "No patients found",
                    200);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<PatientModel>>.Failure(ex.Message,
                    "Internal server error while retrieving patients", 500);
            }
        }

        public async Task<ServiceResult<PatientModel>> GetPatientByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return ServiceResult<PatientModel>.Failure(
                        "Invalid patient ID. ID must be a positive integer",
                        "Invalid request", 400);
                }

                var patient = await _context.Patiens
                    .Include(p => p.Appointments)
                    .ThenInclude(a => a.Doctor)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (patient == null)
                {
                    return ServiceResult<PatientModel>.Failure(
                        $"Patient with ID {id} not found", "Patient not found",404);
                }

                return ServiceResult<PatientModel>.Success(patient,
                    "Patient retrieved successfully",200);
            }
            catch (Exception ex)
            {
                return ServiceResult<PatientModel>.Failure(ex.Message,
                    $"Internal server error: {ex.Message}",500);
            }
        }

        public async Task<ServiceResult<PatientModel>> CreatePatientAsync(PatientDto patientDto)
        {
            try
            {
                if (patientDto == null)
                {
                    return ServiceResult<PatientModel>.Failure(
                        "Patient data is required","Invalid request", 400);
                }

                if (string.IsNullOrWhiteSpace(patientDto.FullName))
                {
                    return ServiceResult<PatientModel>.Failure(
                        "Patient full name is required",
                        "Validation failed", 400);
                }

                if (!string.IsNullOrWhiteSpace(patientDto.Email) && !IsValidEmail(patientDto.Email))
                {
                    return ServiceResult<PatientModel>.Failure(
                        "Valid email address is required","Invalid email format", 400);
                }

                if (!string.IsNullOrWhiteSpace(patientDto.Email))
                {
                    var emailExists = await _context.Patiens
                        .AnyAsync(p => p.Email.ToLower() == patientDto.Email.ToLower());

                    if (emailExists)
                    {
                        return ServiceResult<PatientModel>.Failure(
                            "A patient with this email already exists","Duplicate email",409);
                    }
                }

                if (!string.IsNullOrWhiteSpace(patientDto.Phone) && !IsValidPhone(patientDto.Phone))
                {
                    return ServiceResult<PatientModel>.Failure(
                        "Valid phone number is required (10-15 digits)",
                        "Invalid phone format", 400);
                }

                if (patientDto.DateOfBirth != null)
                {
                    if (patientDto.DateOfBirth > DateTime.Now)
                    {
                        return ServiceResult<PatientModel>.Failure(
                            "Date of birth cannot be in the future",
                            "Invalid date",400);
                    }

                    var age = DateTime.Now.Year - patientDto.DateOfBirth.Year;
                    if (age > 120)
                    {
                        return ServiceResult<PatientModel>.Failure(
                            "Invalid date of birth. Age cannot exceed 120 years",
                            "Invalid date",
                            400);
                    }
                }

                var newPatient = new PatientModel
                {
                    FullName = patientDto.FullName.Trim(),
                    DateOfBirth = patientDto.DateOfBirth,
                    Phone = patientDto.Phone?.Trim(),
                    Email = patientDto.Email?.Trim().ToLower(),
                };

                await _context.Patiens.AddAsync(newPatient);
                await _context.SaveChangesAsync();

                return ServiceResult<PatientModel>.Success(
                    newPatient,
                    "Patient created successfully",201);
            }
            catch (Exception ex)
            {
                return ServiceResult<PatientModel>.Failure(
                    ex.Message,
                    $"Internal server error: {ex.Message}",
                    500);
            }
        }

        public async Task<ServiceResult<PatientModel>> UpdatePatientAsync(int id, PatientDto patientDto)
        {
            try
            {
                if (patientDto == null)
                {
                    return ServiceResult<PatientModel>.Failure(
                        "Patient data is required",
                        "Invalid request",400);
                }

                if (id <= 0)
                {
                    return ServiceResult<PatientModel>.Failure(
                        "Invalid patient ID. ID must be a positive integer",
                        "Invalid request",400);
                }

                var existingPatient = await _context.Patiens
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (existingPatient == null)
                {
                    return ServiceResult<PatientModel>.Failure(
                        $"Patient with ID {id} not found",
                        "Patient not found",
                        404);
                }
                if (!string.IsNullOrWhiteSpace(patientDto.Email) && !IsValidEmail(patientDto.Email))
                {
                    return ServiceResult<PatientModel>.Failure(
                        "Valid email address is required",
                        "Invalid email format",400);
                }

                if (!string.IsNullOrWhiteSpace(patientDto.Email) &&
                    patientDto.Email.ToLower() != existingPatient.Email?.ToLower())
                {
                    var emailExists = await _context.Patiens
                        .AnyAsync(p => p.Email.ToLower() == patientDto.Email.ToLower() && p.Id != id);

                    if (emailExists)
                    {
                        return ServiceResult<PatientModel>.Failure(
                            "Another patient with this email already exists",
                            "Duplicate email",
                            409);
                    }
                }
                if (!string.IsNullOrWhiteSpace(patientDto.Phone) && !IsValidPhone(patientDto.Phone))
                {
                    return ServiceResult<PatientModel>.Failure(
                        "Valid phone number is required (10-15 digits)",
                        "Invalid phone format",
                        400);
                }

                if (patientDto.DateOfBirth != null)
                {
                    if (patientDto.DateOfBirth > DateTime.Now)
                    {
                        return ServiceResult<PatientModel>.Failure(
                            "Date of birth cannot be in the future",
                            "Invalid date",400);
                    }

                    var age = DateTime.Now.Year - patientDto.DateOfBirth.Year;
                    if (age > 120)
                    {
                        return ServiceResult<PatientModel>.Failure(
                            "Invalid date of birth. Age cannot exceed 120 years",
                            "Invalid date",
                            400);
                    }
                }

                if (!string.IsNullOrWhiteSpace(patientDto.FullName))
                    existingPatient.FullName = patientDto.FullName.Trim();

                if (patientDto.DateOfBirth != null)
                    existingPatient.DateOfBirth = patientDto.DateOfBirth;

                if (!string.IsNullOrWhiteSpace(patientDto.Phone))
                    existingPatient.Phone = patientDto.Phone.Trim();


                await _context.SaveChangesAsync();

                return ServiceResult<PatientModel>.Success(
                    existingPatient,
                    "Patient updated successfully", 200);
            }
            catch (Exception ex)
            {
                return ServiceResult<PatientModel>.Failure(
                    ex.Message,
                    $"Internal server error: {ex.Message}",500);
            }
        }

        public async Task<ServiceResult<bool>> DeletePatientAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return ServiceResult<bool>.Failure(
                        "Invalid patient ID. ID must be a positive integer",
                        "Invalid request", 400);
                }

                var patient = await _context.Patiens
                    .Include(p => p.Appointments)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (patient == null)
                {
                    return ServiceResult<bool>.Failure(
                        $"Patient with ID {id} not found","Patient not found", 404);
                }
                var hasActiveAppointments = patient.Appointments != null &&
                    patient.Appointments.Any(a => a.Status != StatusEnum.cancelled && a.Status != StatusEnum.completed);

                if (hasActiveAppointments)
                {
                    return ServiceResult<bool>.Failure(
                        "Cannot delete patient with active appointments. Please cancel or complete appointments first.",
                        "Patient has active appointments",
                        400);
                }

                _context.Patiens.Remove(patient);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Success(true,
                    "Patient deleted successfully",204);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure(ex.Message,
                    $"Internal server error: {ex.Message}", 500);
            }
        }

        public async Task<ServiceResult<List<PatientModel>>> SearchPatientsAsync(SearchPatient searchCriteria)
        {
            try
            {
                if (searchCriteria == null)
                {
                    return ServiceResult<List<PatientModel>>.Failure(
                        "Search criteria is required",
                        "Invalid request",400);
                }

                bool hasName = !string.IsNullOrWhiteSpace(searchCriteria.Name);
                bool hasPhone = !string.IsNullOrWhiteSpace(searchCriteria.Phone);

                if (!hasName && !hasPhone)
                {
                    return ServiceResult<List<PatientModel>>.Failure(
                        "Please provide either a name or a phone number to search for patients",
                        "Invalid search criteria", 400);
                }

                IQueryable<PatientModel> query = _context.Patiens;
                if (hasName)
                {
                    query = query.Where(p => p.FullName.Contains(searchCriteria.Name.Trim(), StringComparison.OrdinalIgnoreCase));
                }
                if (hasPhone)
                {
                    query = query.Where(p => p.Phone.Contains(searchCriteria.Phone.Trim()));
                }

                var patients = await query.OrderBy(p => p.FullName).ToListAsync();

                if (!patients.Any())
                {
                    string searchTerm = hasName ? $"name '{searchCriteria.Name}'" : $"phone number '{searchCriteria.Phone}'";
                    return ServiceResult<List<PatientModel>>.Success(
                        new List<PatientModel>(),
                        $"No patients found with {searchTerm}",200);
                }

                return ServiceResult<List<PatientModel>>.Success(
                    patients,
                    $"Found {patients.Count} patient(s) matching your search criteria",200);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<PatientModel>>.Failure(ex.Message,
                    "An error occurred while searching for patients",500);
            }
        }

        public async Task<ServiceResult<List<AppointmentModel>>> GetPatientAppointmentsAsync(int patientId)
        {
            try
            {
                if (patientId <= 0)
                {
                    return ServiceResult<List<AppointmentModel>>.Failure(
                        "Invalid patient ID. ID must be a positive integer",
                        "Invalid request",
                        400);
                }

                var patient = await _context.Patiens
                    .FirstOrDefaultAsync(p => p.Id == patientId);

                if (patient == null)
                {
                    return ServiceResult<List<AppointmentModel>>.Failure(
                        $"Patient with ID {patientId} not found",
                        "Patient not found",
                        404);
                }

                var appointments = await _context.Appointments
                    .Where(a => a.PatientId == patientId)
                    .Include(a => a.Doctor)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToListAsync();

                return ServiceResult<List<AppointmentModel>>.Success(
                    appointments,
                    appointments.Any() ? $"Retrieved {appointments.Count} appointments for patient" : "No appointments found for this patient",
                    200);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<AppointmentModel>>.Failure(
                    ex.Message,
                    $"Internal server error: {ex.Message}",
                    500);
            }
        }

        public async Task<ServiceResult<bool>> ValidatePatientEmailAsync(string email, int? excludePatientId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return ServiceResult<bool>.Failure(
                        "Email is required", "Invalid request",400);
                }

                if (!IsValidEmail(email))
                {
                    return ServiceResult<bool>.Failure(
                        "Invalid email format",
                        "Validation failed", 400);
                }

                var query = _context.Patiens.Where(p => p.Email.ToLower() == email.ToLower());

                if (excludePatientId.HasValue)
                {
                    query = query.Where(p => p.Id != excludePatientId.Value);
                }

                var exists = await query.AnyAsync();

                if (exists)
                {
                    return ServiceResult<bool>.Failure(
                        "Email already exists",
                        "Duplicate email",409);
                }

                return ServiceResult<bool>.Success(true, "Email is available", 200);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure(ex.Message,
                    $"Internal server error: {ex.Message}",500);
            }
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;

                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            var digits = new string(phone.Where(char.IsDigit).ToArray());

            return digits.Length >= 10 && digits.Length <= 15;
        }
    }
}