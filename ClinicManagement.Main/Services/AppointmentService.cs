using ClinicAppointmentHR.Data;
using ClinicAppointmentHR.Dtos.AppointmentDtos;
using ClinicAppointmentHR.Models;
using ClinicManagement.App.Dtos.BonusDtos;
using ClinicManagement.Main.Results;
using ClinicManagementSystem.App.Dtos.AppointmentDtos;
using ClinicManagementSystem.App.Enums;
using ClinicManagementSystem.App.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.App.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppDbContext _context;

        public AppointmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<AppointmentModel>> ScheduleAppointmentAsync(ScheduletAppointmentDto schedule)
        {
            try
            {
                if (schedule.AppointmentDate < DateTime.Now)
                {
                    return ServiceResult<AppointmentModel>.Failure(
                        "Appointment date must be in the future.", "Invalid appointment date",400);
                }

                var doctor = await _context.Doctors.FindAsync(schedule.DoctorId);
                if (doctor == null)
                {
                    return ServiceResult<AppointmentModel>.Failure(
                        "Doctor not found","Doctor does not exist", 404);
                }

                var patient = await _context.Patiens.FindAsync(schedule.PatientId);
                if (patient == null)
                {
                    return ServiceResult<AppointmentModel>.Failure(
                        "Patient not found","Patient does not exist",404);
                }

                var hasConflict = await _context.Appointments
                    .AnyAsync(a => a.DoctorId == schedule.DoctorId && a.AppointmentDate == schedule.AppointmentDate
                    && a.Status != StatusEnum.cancelled);

                if (hasConflict)
                {
                    return ServiceResult<AppointmentModel>.Failure(
                        "Doctor already has an active appointment at this date and time",
                        "Schedule conflict",
                        409);
                }

                var appointment = new AppointmentModel
                {
                    DoctorId = schedule.DoctorId,
                    PatientId = schedule.PatientId,
                    AppointmentDate = schedule.AppointmentDate,
                    Status = StatusEnum.completed,
                    VisitType = VisitTypeEnum.RoutineCheckup
                };

                await _context.Appointments.AddAsync(appointment);
                await _context.SaveChangesAsync();

                return ServiceResult<AppointmentModel>.Success(
                    appointment,
                    "Appointment scheduled successfully", 200);
            }
            catch (Exception ex)
            {
                return ServiceResult<AppointmentModel>.Failure(
                    ex.Message,
                    "An error occurred while scheduling the appointment",500);
            }
        }

        public async Task<ServiceResult<AppointmentModel>> UpdateAppointmentAsync(int id, UpdateAppoitmentDto update)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment is null)
                {
                    return ServiceResult<AppointmentModel>.Failure(
                        "Appointment not found","Appointment does not exist", 404);
                }

                if (appointment.Status == StatusEnum.cancelled)
                {
                    return ServiceResult<AppointmentModel>.Failure(
                        "Cannot update a cancelled appointment","Invalid operation", 400);
                }

                if (update.AppointmentDate < DateTime.Now)
                {
                    return ServiceResult<AppointmentModel>.Failure(
                        "Appointment date must be in the future","Invalid date",400);
                }

                var hasConflict = await _context.Appointments
                    .AnyAsync(a => a.DoctorId == appointment.DoctorId && a.AppointmentDate == update.AppointmentDate
                        && a.Id != id
                        && a.Status != StatusEnum.cancelled);

                if (hasConflict)
                {
                    return ServiceResult<AppointmentModel>.Failure(
                        "Doctor already has an active appointment at this date and time","Schedule conflict",400);
                }

                appointment.AppointmentDate = update.AppointmentDate;
                appointment.Status = StatusEnum.Update;

                await _context.SaveChangesAsync();

                return ServiceResult<AppointmentModel>.Success(appointment,
                    "Appointment updated successfully",200);
            }
            catch (Exception ex)
            {
                return ServiceResult<AppointmentModel>.Failure(
                    ex.Message,"An error occurred while updating the appointment",500);
            }
        }

        public async Task<ServiceResult<bool>> CancelAppointmentAsync(int id)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment is null)
                {
                    return ServiceResult<bool>.Failure(
                        "Appointment not found", "Appointment does not exist",404);
                }

                if (appointment.Status == StatusEnum.cancelled)
                {
                    return ServiceResult<bool>.Failure(
                        "Appointment is already cancelled", "Invalid operation", 400);
                }

                if (appointment.Status == StatusEnum.completed)
                {
                    return ServiceResult<bool>.Failure(
                        "Cannot cancel a completed appointment","Invalid operation", 400);
                }

                appointment.Status = StatusEnum.cancelled;
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Success(true,
                    "Appointment cancelled successfully", 200);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure(ex.Message,
                    "An error occurred while cancelling the appointment", 500);
            }
        }

        public async Task<ServiceResult<List<AppointmentModel>>> GetCompletedAppointmentsAsync()
        {
            try
            {
                var appointments = await _context.Appointments
                    .Where(a => a.Status == StatusEnum.completed)
                    .Include(a => a.Doctor)
                    .Include(a => a.Patient)
                    .ToListAsync();

                return ServiceResult<List<AppointmentModel>>.Success(appointments,
                    "Completed appointments retrieved successfully",200);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<AppointmentModel>>.Failure(ex.Message,
                    "An error occurred while retrieving completed appointments", 500);
            }
        }

        public async Task<ServiceResult<List<AppointmentModel>>> GetAppointmentsByDateAndDoctorAsync(getAppointmentByDotorAndDateDto appointment)
        {
            try
            {
                var appointments = await _context.Appointments
                    .Where(a => a.AppointmentDate.Date == appointment.AppointmentDate
                        && a.DoctorId == appointment.DoctorId)
                    .Include(a => a.Doctor)
                    .Include(a => a.Patient)
                    .ToListAsync();

                if (appointments == null || appointments.Count == 0)
                {
                    return ServiceResult<List<AppointmentModel>>.Success(
                        new List<AppointmentModel>(),
                        "No appointments found for the specified date and doctor",200);
                }

                return ServiceResult<List<AppointmentModel>>.Success(
                    appointments,"Appointments retrieved successfully",
                    200);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<AppointmentModel>>.Failure(
                    ex.Message,
                    "An error occurred while searching for doctor and date",
                    500);
            }
        }

        public async Task<ServiceResult<List<AppointmentModel>>> GetTodayAppointmentsAsync()
        {
            try
            {
                var todayAppointments = await _context.Appointments
                    .Where(a => a.AppointmentDate.Date == DateTime.Now.Date)
                    .Include(a => a.Doctor)
                    .Include(a => a.Patient)
                    .ToListAsync();

                return ServiceResult<List<AppointmentModel>>.Success(
                    todayAppointments, "Today's appointments retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<AppointmentModel>>.Failure(
                    ex.Message, "An error occurred while retrieving today's appointments", 500);
            }
        }

        public async Task<ServiceResult<List<AppointmentModel>>> GetAllAppointmentsAsync()
        {
            try
            {
                var appointments = await _context.Appointments
                    .Include(a => a.Doctor)
                    .Include(a => a.Patient)
                    .ToListAsync();

                return ServiceResult<List<AppointmentModel>>.Success(
                    appointments,"All appointments retrieved successfully",200);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<AppointmentModel>>.Failure(
                    ex.Message,"Internal server error",500);
            } 
        }

        public async Task<ServiceResult<AppointmentModel>> GetAppointmentByIdAsync(int id)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Doctor)
                    .Include(a => a.Patient)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (appointment == null)
                {
                    return ServiceResult<AppointmentModel>.Failure(
                        "Appointment not found", "Appointment does not exist", 404);
                }

                return ServiceResult<AppointmentModel>.Success(
                    appointment,"Appointment retrieved successfully",200);
            }
            catch (Exception ex)
            {
                return ServiceResult<AppointmentModel>.Failure(
                    ex.Message,
                    $"Internal server error: {ex.Message}",
                    500);
            }
        }

        public async Task<ServiceResult<AppointmentModel>> CreateAppointmentAsync(AppointmentDto appointment)
        {
            try
            {
                if (appointment == null)
                {
                    return ServiceResult<AppointmentModel>.Failure(
                        "Appointment data is required", "Invalid request", 400);
                }

                var existingAppointment = new AppointmentModel
                {
                    DoctorId = appointment.DoctorId,
                    PatientId = appointment.PatientId,
                    AppointmentDate = appointment.AppointmentDate,
                    Status = StatusEnum.completed,
                    Notes = appointment.Notes,
                    VisitType = VisitTypeEnum.RoutineCheckup
                };

                await _context.Appointments.AddAsync(existingAppointment);
                await _context.SaveChangesAsync();

                return ServiceResult<AppointmentModel>.Success(
                    existingAppointment, "Appointment created successfully", 200);
            }
            catch (Exception ex)
            {
                return ServiceResult<AppointmentModel>.Failure(ex.Message,
                    $"Internal server error: {ex.Message}", 500);
            }
        }

        public async Task<ServiceResult<bool>> DeleteAppointmentAsync(int id)
        {
            try
            {
                var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id);
                if (appointment == null)
                {
                    return ServiceResult<bool>.Failure(
                        "Appointment not found", "Appointment does not exist", 404);
                }

                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Success(true,
                    "Appointment deleted successfully",204);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure(
                    ex.Message,$"Internal server error: {ex.Message}",
                    500);
            }
        }

        public async Task<ServiceResult<object>> GetFilteredAppointmentsAsync(AppointmentFilterDto filter)
        {
            try
            {
                var query = _context.Appointments
                    .Include(a => a.Doctor)
                    .Include(a => a.Patient)
                    .AsQueryable();

                if (filter.DoctorId.HasValue)
                    query = query.Where(a => a.DoctorId == filter.DoctorId);

                if (filter.PatientId.HasValue)
                    query = query.Where(a => a.PatientId == filter.PatientId);

                if (!string.IsNullOrEmpty(filter.Status))
                    query = query.Where(a => a.Status.ToString() == filter.Status);

                if (filter.FromDate.HasValue)
                    query = query.Where(a => a.AppointmentDate >= filter.FromDate);

                if (filter.ToDate.HasValue)
                    query = query.Where(a => a.AppointmentDate <= filter.ToDate);

                var total = await query.CountAsync();
                var items = await query
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(a => new
                    {
                        a.Id,
                        a.AppointmentDate,
                        DoctorName = a.Doctor.FullName,
                        PatientName = a.Patient.FullName,
                        a.Status,
                        a.VisitType
                    })
                    .ToListAsync();

                var objectItems = items.Cast<object>().ToList();
                var totalCount = await query.CountAsync();

                var result = new PagedResult<object>
                {
                    Items = objectItems,
                    TotalCount = totalCount,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };

                return ServiceResult<object>.Success(result, "Appointments retrieved", 200);
            }
            catch (Exception ex)
            {
                return ServiceResult<object>.Failure(ex.Message, "Error", 500);
            }
        }
    }
}