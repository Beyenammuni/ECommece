using ClinicManagementSystem.App.Enums;

namespace ClinicAppointmentHR.Models
{
    public class PaymentModel
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethodEnum Method { get; set; }
        public PaymentStatusEnum Status { get; set; }
        public AppointmentModel Appointment { get; set; } = new AppointmentModel();
    }
}
