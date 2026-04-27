using ClinicManagementSystem.App.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicManagementSystem.App.Dtos.PaymentDtos
{
    public class UnpaidAppointmentDto
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal? ExpectedAmount { get; set; }

    }
}
