using ClinicAppointmentHR.Dtos.PaymentDtos;
using ClinicManagement.App.Dtos.PaymentDtos;
using ClinicManagement.Main.Results;
using ClinicManagementSystem.App.Dtos.PaymentDtos;
using ClinicManagementSystem.App.Enums;

namespace ClinicAppointment.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<ServiceResult<List<AppointmentPaymentDto>>> GetPaymentsForAppointmentAsync(int appointmentId);
        Task<ServiceResult<List<UnpaidAppointmentDto>>> GetUnpaidCompletedAppointmentsAsync();
        Task<ServiceResult<PaymentDto>> ProcessPaymentAsync(CreatePaymentDto paymentDto);
        Task<ServiceResult<PaymentDto>> GetPaymentByIdAsync(int paymentId);
        Task<ServiceResult<List<PaymentDto>>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ServiceResult<decimal>> GetTotalPaymentsForAppointmentAsync(int appointmentId);
        Task<ServiceResult<PaymentSummaryDto>> GetPaymentSummaryAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<ServiceResult<bool>> RefundPaymentAsync(int paymentId);
        Task<ServiceResult<bool>> CheckAppointmentPaymentStatusAsync(int appointmentId);
        Task<ServiceResult<MonthlyPaymentSummaryDto>> GetMonthlyPaymentSummaryAsync(int year, int month);
    }
}

