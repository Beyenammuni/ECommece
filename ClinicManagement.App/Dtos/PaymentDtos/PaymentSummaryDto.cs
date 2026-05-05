using ClinicManagementSystem.App.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicManagement.App.Dtos.PaymentDtos
{
    public class PaymentSummaryDto
    {
        public decimal TotalPayments { get; set; }
        public int TotalTransactions { get; set; }
        public Dictionary<PaymentMethodEnum, decimal> PaymentsByMethod { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public List<DailyPaymentDto> DailyBreakdown { get; set; }
    }
}
