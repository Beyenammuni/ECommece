namespace ClinicAppointment.Services.Interfaces
{
    public class MonthlyPaymentSummaryDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalPayments { get; set; }
        public Dictionary<string, decimal> PaymentsByMethod { get; set; }
    }
}