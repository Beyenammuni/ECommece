using ClinicManagement.App.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicManagement.App.Dtos.DoctorDtos
{
    public class WorkingHoursDto
    {
        public DayOfWeekEnum DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
