using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicManagement.App.Dtos.PaymentDtos
{
    public class DailyPaymentDto
    {
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public int TransactionCount { get; set; }
    }
}
