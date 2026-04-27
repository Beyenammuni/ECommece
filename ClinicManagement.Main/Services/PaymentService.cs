using ClinicAppointment.Services.Interfaces;
using ClinicAppointmentHR.Data;
using ClinicAppointmentHR.Dtos.PaymentDtos;
using ClinicAppointmentHR.Models;
using ClinicManagement.App.Dtos.PaymentDtos;
using ClinicManagement.Main.Results;
using ClinicManagementSystem.App.Dtos.PaymentDtos;
using ClinicManagementSystem.App.Enums;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointment.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<AppointmentPaymentDto>>> GetPaymentsForAppointmentAsync(int appointmentId)
        {
            try
            {
                if (appointmentId <= 0)
                {
                    return ServiceResult<List<AppointmentPaymentDto>>.Failure(
                        "Invalid appointment ID. ID must be a positive integer",
                        "Invalid request",400);
                }

                var appointmentExists = await _context.Appointments
                    .AnyAsync(a => a.Id == appointmentId);

                if (!appointmentExists)
                {
                    return ServiceResult<List<AppointmentPaymentDto>>.Failure(
                        $"Appointment with ID {appointmentId} not found",
                        "Appointment not found", 404);
                }

                var payments = await _context.Payments
                    .Where(p => p.AppointmentId == appointmentId)
                    .Select(p => new AppointmentPaymentDto
                    {
                        AppointmentId = p.AppointmentId,
                        Amount = p.Amount,
                        PaymentDate = p.PaymentDate,
                        PaymentMethod = p.Method,
                        Status = p.Status,
                    })
                    .OrderByDescending(p => p.PaymentDate)
                    .ToListAsync();

                return ServiceResult<List<AppointmentPaymentDto>>.Success(
                    payments,
                    payments.Any()
                        ? $"Retrieved {payments.Count} payment(s) for appointment {appointmentId}"
                        : $"No payments found for appointment {appointmentId}",
                    200);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<AppointmentPaymentDto>>.Failure(
                    ex.Message,
                    "An error occurred while retrieving payments",
                    500);
            }
        }

        public async Task<ServiceResult<List<UnpaidAppointmentDto>>> GetUnpaidCompletedAppointmentsAsync()
        {
            try
            {
                var unpaidAppointments = await _context.Appointments
                    .Where(a => a.Status == StatusEnum.completed &&
                           !_context.Payments.Any(p => p.AppointmentId == a.Id))
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Select(a => new UnpaidAppointmentDto
                    {
                        AppointmentId = a.Id,
                        PatientName = a.Patient.FullName,
                        DoctorName = a.Doctor.FullName,
                        AppointmentDate = a.AppointmentDate,
                    })
                    .OrderBy(a => a.AppointmentDate)
                    .ToListAsync();

                return ServiceResult<List<UnpaidAppointmentDto>>.Success(
                    unpaidAppointments,
                    unpaidAppointments.Any()
                        ? $"Found {unpaidAppointments.Count} unpaid completed appointment(s)"
                        : "No unpaid completed appointments found",
                    200);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<UnpaidAppointmentDto>>.Failure(
                    ex.Message,
                    "An error occurred while retrieving unpaid appointments",
                    500);
            }
        }

        public async Task<ServiceResult<PaymentDto>> ProcessPaymentAsync(CreatePaymentDto paymentDto)
        {
            try
            {
                if (paymentDto == null)
                {
                    return ServiceResult<PaymentDto>.Failure(
                        "Payment data is required",
                        "Invalid request",
                        400);
                }

                var appointment = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.Id == paymentDto.AppointmentId);

                if (appointment == null)
                {
                    return ServiceResult<PaymentDto>.Failure(
                        $"Appointment with ID {paymentDto.AppointmentId} not found",
                        "Appointment not found",
                        404);
                }

                if (appointment.Status != StatusEnum.completed)
                {
                    return ServiceResult<PaymentDto>.Failure(
                        "Payment can only be processed for completed appointments",
                        "Invalid appointment status",
                        400);
                }

                var existingPayment = await _context.Payments
                    .FirstOrDefaultAsync(p => p.AppointmentId == paymentDto.AppointmentId);

                if (existingPayment != null)
                {
                    return ServiceResult<PaymentDto>.Failure(
                        "Payment already processed for this appointment",
                        "Duplicate payment",
                        409);
                }

                var payment = new PaymentModel
                {
                    AppointmentId = paymentDto.AppointmentId,
                    Amount = paymentDto.Amount,
                    PaymentDate = DateTime.UtcNow,
                    Method = paymentDto.PaymentMethod,
                    Status = PaymentStatusEnum.Completed,
                };

                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();

                var result = new PaymentDto
                {
                    Id = payment.Id,
                    AppointmentId = payment.AppointmentId,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    PaymentMethod = payment.Method,
                    Status = payment.Status,
                };

                return ServiceResult<PaymentDto>.Success(
                    result,
                    "Payment processed successfully",
                    201);
            }
            catch (Exception ex)
            {
                return ServiceResult<PaymentDto>.Failure(
                    ex.Message,
                    $"An error occurred while processing payment: {ex.Message}",
                    500);
            }
        }

        public async Task<ServiceResult<PaymentDto>> GetPaymentByIdAsync(int paymentId)
        {
            try
            {
                if (paymentId <= 0)
                {
                    return ServiceResult<PaymentDto>.Failure(
                        "Invalid payment ID. ID must be a positive integer",
                        "Invalid request",
                        400);
                }

                var payment = await _context.Payments
                    .Include(p => p.Appointment)
                    .ThenInclude(a => a.Patient)
                    .Include(p => p.Appointment)
                    .ThenInclude(a => a.Doctor)
                    .FirstOrDefaultAsync(p => p.Id == paymentId);

                if (payment == null)
                {
                    return ServiceResult<PaymentDto>.Failure(
                        $"Payment with ID {paymentId} not found",
                        "Payment not found",
                        404);
                }

                var result = new PaymentDto
                {
                    Id = payment.Id,
                    AppointmentId = payment.AppointmentId,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    PaymentMethod = payment.Method,
                    Status = payment.Status,

                };

                return ServiceResult<PaymentDto>.Success(
                    result,
                    "Payment retrieved successfully",
                    200);
            }
            catch (Exception ex)
            {
                return ServiceResult<PaymentDto>.Failure(
                    ex.Message,
                    $"Internal server error: {ex.Message}",
                    500);
            }
        }

        public async Task<ServiceResult<List<PaymentDto>>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                {
                    return ServiceResult<List<PaymentDto>>.Failure(
                        "Start date must be before or equal to end date",
                        "Invalid date range",
                        400);
                }

                var payments = await _context.Payments
                    .Where(p => p.PaymentDate.Date >= startDate.Date &&
                           p.PaymentDate.Date <= endDate.Date)
                    .Include(p => p.Appointment)
                    .OrderByDescending(p => p.PaymentDate)
                    .Select(p => new PaymentDto
                    {
                        Id = p.Id,
                        AppointmentId = p.AppointmentId,
                        Amount = p.Amount,
                        PaymentDate = p.PaymentDate,
                        PaymentMethod = p.Method,
                        Status = p.Status,

                    })
                    .ToListAsync();

                return ServiceResult<List<PaymentDto>>.Success(
                    payments,
                    $"Retrieved {payments.Count} payment(s) from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}",
                    200);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<PaymentDto>>.Failure(
                    ex.Message,
                    $"Internal server error: {ex.Message}",
                    500);
            }
        }

        public async Task<ServiceResult<decimal>> GetTotalPaymentsForAppointmentAsync(int appointmentId)
        {
            try
            {
                if (appointmentId <= 0)
                {
                    return ServiceResult<decimal>.Failure(
                        "Invalid appointment ID. ID must be a positive integer",
                        "Invalid request",
                        400);
                }

                var total = await _context.Payments
                    .Where(p => p.AppointmentId == appointmentId && p.Status == PaymentStatusEnum.Completed)
                    .SumAsync(p => p.Amount);

                return ServiceResult<decimal>.Success(
                    total,
                    $"Total payments for appointment {appointmentId}: {total:C}",
                    200);
            }
            catch (Exception ex)
            {
                return ServiceResult<decimal>.Failure(
                    ex.Message,
                    $"Internal server error: {ex.Message}",
                    500);
            }
        }

        public async Task<ServiceResult<PaymentSummaryDto>> GetPaymentSummaryAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var end = endDate ?? DateTime.UtcNow.Date;
                var start = startDate ?? end.AddDays(-30);

                if (start > end)
                {
                    return ServiceResult<PaymentSummaryDto>.Failure(
                        "Start date must be before or equal to end date",
                        "Invalid date range",
                        400);
                }

                var payments = await _context.Payments
                    .Where(p => p.PaymentDate.Date >= start.Date &&
                           p.PaymentDate.Date <= end.Date &&
                           p.Status == PaymentStatusEnum.Completed)
                    .ToListAsync();

                var totalPayments = payments.Sum(p => p.Amount);
                var paymentsByMethod = payments
                    .GroupBy(p => p.Method)
                    .ToDictionary(g => g.Key, g => g.Sum(p => p.Amount));

                var dailyBreakdown = payments
                    .GroupBy(p => p.PaymentDate.Date)
                    .Select(g => new DailyPaymentDto
                    {
                        Date = g.Key,
                        TotalAmount = g.Sum(p => p.Amount),
                        TransactionCount = g.Count()
                    })
                    .OrderBy(d => d.Date)
                    .ToList();

                var summary = new PaymentSummaryDto
                {
                    TotalPayments = totalPayments,
                    TotalTransactions = payments.Count,
                    PaymentsByMethod = paymentsByMethod,
                    PeriodStart = start,
                    PeriodEnd = end,
                    DailyBreakdown = dailyBreakdown
                };

                return ServiceResult<PaymentSummaryDto>.Success(
                    summary,
                    $"Payment summary retrieved for period {start:yyyy-MM-dd} to {end:yyyy-MM-dd}",
                    200);
            }
            catch (Exception ex)
            {
                return ServiceResult<PaymentSummaryDto>.Failure(
                    ex.Message,
                    $"Internal server error: {ex.Message}",
                    500);
            }
        }

        public async Task<ServiceResult<bool>> RefundPaymentAsync(int paymentId)
        {
            try
            {
                if (paymentId <= 0)
                {
                    return ServiceResult<bool>.Failure(
                        "Invalid payment ID. ID must be a positive integer",
                        "Invalid request",
                        400);
                }

                var payment = await _context.Payments
                    .FirstOrDefaultAsync(p => p.Id == paymentId);

                if (payment == null)
                {
                    return ServiceResult<bool>.Failure(
                        $"Payment with ID {paymentId} not found",
                        "Payment not found",
                        404);
                }

                if (payment.Status == PaymentStatusEnum.Refunded)
                {
                    return ServiceResult<bool>.Failure(
                        "Payment has already been refunded",
                        "Duplicate refund",
                        400);
                }

                if (payment.Status != PaymentStatusEnum.Completed)
                {
                    return ServiceResult<bool>.Failure(
                        "Only completed payments can be refunded",
                        "Invalid payment status",
                        400);
                }

                payment.Status = PaymentStatusEnum.Refunded;

                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Success(
                    true,
                    "Payment refunded successfully",
                    200);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure(
                    ex.Message,
                    $"Internal server error: {ex.Message}",
                    500);
            }
        }

        public async Task<ServiceResult<bool>> CheckAppointmentPaymentStatusAsync(int appointmentId)
        {
            try
            {
                if (appointmentId <= 0)
                {
                    return ServiceResult<bool>.Failure(
                        "Invalid appointment ID. ID must be a positive integer",
                        "Invalid request",
                        400);
                }

                var hasPayment = await _context.Payments
                    .AnyAsync(p => p.AppointmentId == appointmentId &&
                             p.Status == PaymentStatusEnum.Completed);

                return ServiceResult<bool>.Success(
                    hasPayment,
                    hasPayment
                        ? $"Appointment {appointmentId} has been paid"
                        : $"Appointment {appointmentId} has not been paid",
                    200);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure(
                    ex.Message,
                    $"Internal server error: {ex.Message}",
                    500);
            }
        }
        public async Task<ServiceResult<MonthlyPaymentSummaryDto>> GetMonthlyPaymentSummaryAsync(int year, int month)
        {
            try
            {
                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var payments = await _context.Payments
                    .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                    .ToListAsync();

                var summary = new MonthlyPaymentSummaryDto
                {
                    Year = year,
                    Month = month,
                    MonthName = startDate.ToString("MMMM"),
                    TotalAmount = payments.Sum(p => p.Amount),
                    TotalPayments = payments.Count,
                    PaymentsByMethod = payments
                        .GroupBy(p => p.Method.ToString())
                        .ToDictionary(g => g.Key, g => g.Sum(p => p.Amount))
                };

                return ServiceResult<MonthlyPaymentSummaryDto>.Success(summary, "Monthly summary retrieved", 200);
            }
            catch (Exception ex)
            {
                return ServiceResult<MonthlyPaymentSummaryDto>.Failure(ex.Message, "Error", 500);
            }
        }

        private string GenerateTransactionId()
        {
            return $"TXN_{DateTime.UtcNow:yyyyMMddHHmmss}_{new Random().Next(1000, 9999)}";
        }

       
    }
}