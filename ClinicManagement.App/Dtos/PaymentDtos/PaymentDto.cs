using ClinicManagementSystem.App.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicAppointmentHR.Dtos.PaymentDtos
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public PaymentStatusEnum Status { get; set; }
        public string TransactionId { get; set; }
        public string Notes { get; set; }
    }
}
