using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicManagement.App.Dtos.BonusDtos
{
    public class AppointmentFilterDto:PagenationDto
    {
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
        public string? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
