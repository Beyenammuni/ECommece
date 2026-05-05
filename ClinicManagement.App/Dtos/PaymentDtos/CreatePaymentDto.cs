using ClinicManagementSystem.App.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClinicManagement.App.Dtos.PaymentDtos
{
    public class CreatePaymentDto
    {
        [Required(ErrorMessage = "Appointment ID is required")]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        public PaymentMethodEnum PaymentMethod { get; set; }

    }
}
