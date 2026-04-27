using ClinicAppointmentHR.Models;
using ClinicManagement.App.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicManagement.App.Dtos.DoctorDtos
{
    public class DoctorWorkingHours
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public DayOfWeekEnum DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual DoctorModel Doctor { get; set; }
    }
}
