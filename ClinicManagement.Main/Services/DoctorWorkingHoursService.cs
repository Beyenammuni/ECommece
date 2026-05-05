using ClinicAppointmentHR.Data;
using ClinicManagement.App.Dtos.DoctorDtos;
using ClinicManagement.Main.IServices;
using ClinicManagement.Main.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicManagement.Main.Services
{
    namespace ClinicAppointment.Services.Implementations
    {
        public class DoctorWorkingHoursService : IDoctorWorkingHoursService
        {
            private readonly AppDbContext _context;

            public DoctorWorkingHoursService(AppDbContext context)
            {
                _context = context;
            }

            public async Task<ServiceResult<List<DoctorWorkingHours>>> GetDoctorWorkingHoursAsync(int doctorId)
            {
                try
                {
                    var hours = await _context.DoctorWorkingHours
                        .Where(h => h.DoctorId == doctorId && h.IsActive)
                        .OrderBy(h => h.DayOfWeek)
                        .ToListAsync();

                    return ServiceResult<List<DoctorWorkingHours>>.Success(hours, "Working hours retrieved", 200);
                }
                catch (Exception ex)
                {
                    return ServiceResult<List<DoctorWorkingHours>>.Failure(ex.Message, "Error", 500);
                }
            }

            public async Task<ServiceResult<DoctorWorkingHours>> AddWorkingHoursAsync(int doctorId, WorkingHoursDto dto)
            {
                try
                {
                    var doctor = await _context.Doctors.FindAsync(doctorId);
                    if (doctor == null)
                        return ServiceResult<DoctorWorkingHours>.Failure("Doctor not found", "Not found", 404);

                    var existing = await _context.DoctorWorkingHours
                        .FirstOrDefaultAsync(h => h.DoctorId == doctorId && h.DayOfWeek == dto.DayOfWeek && h.IsActive);

                    if (existing != null)
                        return ServiceResult<DoctorWorkingHours>.Failure("Working hours already exist for this day", "Duplicate", 400);

                    var workingHours = new DoctorWorkingHours
                    {
                        DoctorId = doctorId,
                        DayOfWeek = dto.DayOfWeek,
                        StartTime = dto.StartTime,
                        EndTime = dto.EndTime,
                        IsActive = true
                    };

                    await _context.DoctorWorkingHours.AddAsync(workingHours);
                    await _context.SaveChangesAsync();

                    return ServiceResult<DoctorWorkingHours>.Success(workingHours, "Working hours added", 201);
                }
                catch (Exception ex)
                {
                    return ServiceResult<DoctorWorkingHours>.Failure(ex.Message, "Error", 500);
                }
            }

            public async Task<ServiceResult<bool>> UpdateWorkingHoursAsync(int id, WorkingHoursDto dto)
            {
                try
                {
                    var workingHours = await _context.DoctorWorkingHours.FindAsync(id);
                    if (workingHours == null)
                        return ServiceResult<bool>.Failure("Working hours not found", "Not found", 404);

                    workingHours.StartTime = dto.StartTime;
                    workingHours.EndTime = dto.EndTime;
                    await _context.SaveChangesAsync();

                    return ServiceResult<bool>.Success(true, "Working hours updated", 200);
                }
                catch (Exception ex)
                {
                    return ServiceResult<bool>.Failure(ex.Message, "Error", 500);
                }
            }

            public async Task<ServiceResult<bool>> DeleteWorkingHoursAsync(int id)
            {
                try
                {
                    var workingHours = await _context.DoctorWorkingHours.FindAsync(id);
                    if (workingHours == null)
                        return ServiceResult<bool>.Failure("Working hours not found", "Not found", 404);

                    workingHours.IsActive = false;
                    await _context.SaveChangesAsync();

                    return ServiceResult<bool>.Success(true, "Working hours deleted", 200);
                }
                catch (Exception ex)
                {
                    return ServiceResult<bool>.Failure(ex.Message, "Error", 500);
                }
            }

            public async Task<ServiceResult<bool>> ValidateAppointmentTimeAsync(int doctorId, DateTime appointmentDate)
            {
                try
                {
                    var dayOfWeek = appointmentDate.DayOfWeek.ToString();
                    var timeOfDay = appointmentDate.TimeOfDay;

                    var workingHours = await _context.DoctorWorkingHours
                        .FirstOrDefaultAsync(h => h.DoctorId == doctorId && h.DayOfWeek.ToString() == dayOfWeek && h.IsActive);

                    if (workingHours == null)
                        return ServiceResult<bool>.Failure("Doctor not available on this day", "Not available", 400);

                    if (timeOfDay < workingHours.StartTime || timeOfDay > workingHours.EndTime)
                        return ServiceResult<bool>.Failure($"Doctor works from {workingHours.StartTime} to {workingHours.EndTime}", "Outside working hours", 400);

                    return ServiceResult<bool>.Success(true, "Time is valid", 200);
                }
                catch (Exception ex)
                {
                    return ServiceResult<bool>.Failure(ex.Message, "Error", 500);
                }
            }
        }
    }
}
