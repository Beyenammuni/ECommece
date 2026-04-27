using ClinicManagement.App.Dtos.DoctorDtos;
using ClinicManagement.Main.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicManagement.Main.IServices
{
        public interface IDoctorWorkingHoursService
        {
            Task<ServiceResult<List<DoctorWorkingHours>>> GetDoctorWorkingHoursAsync(int doctorId);
            Task<ServiceResult<DoctorWorkingHours>> AddWorkingHoursAsync(int doctorId, WorkingHoursDto dto);
            Task<ServiceResult<bool>> UpdateWorkingHoursAsync(int id, WorkingHoursDto dto);
            Task<ServiceResult<bool>> DeleteWorkingHoursAsync(int id);
            Task<ServiceResult<bool>> ValidateAppointmentTimeAsync(int doctorId, DateTime appointmentDate);
        }
    }
