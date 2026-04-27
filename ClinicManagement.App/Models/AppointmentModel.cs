using ClinicManagementSystem.App.Enums;
using ClinicManagementSystem.App.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicAppointmentHR.Models
{
    public class AppointmentModel
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public StatusEnum Status { get; set; }
        public VisitTypeEnum VisitType { get; set; } 
        public string Notes { get; set; }

        public DoctorModel Doctor { get; set; }  
        public PatientModel Patient { get; set; }
        public PaymentModel Payment { get; set; }
    }

}
