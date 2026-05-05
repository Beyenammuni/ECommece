using ClinicManagementSystem.App.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicManagementSystem.App.Dtos.PaymentDtos
{
    public class AppointmentPaymentDto
    {
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public PaymentStatusEnum Status { get; set; }
        public string TransactionId { get; set; }
    }
}
