using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicManagement.App.Dtos.PaymentDtos
{
    public class ProcessPaymnetDto
    {
        public bool IsSuccessful { get; set; }
        public int PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }
    }
}
