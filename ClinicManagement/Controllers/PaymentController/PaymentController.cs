using ClinicAppointment.Services.Interfaces;
using ClinicManagement.App.Dtos.PaymentDtos;
using ClinicManagementSystem.App.Dtos.PaymentDtos;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Controllers.PaymentController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("Appointment/{appointmentId}")]
        public async Task<IActionResult> GetPaymentsForAppointment(int appointmentId)
        {
            var result = await _paymentService.GetPaymentsForAppointmentAsync(appointmentId);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Data
            });
        }

        [HttpGet("UnpaidAppointments")]
        public async Task<IActionResult> GetUnpaidCompletedAppointments()
        {
            var result = await _paymentService.GetUnpaidCompletedAppointmentsAsync();
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Data
            });
        }


        [HttpPost("Process")]
        public async Task<IActionResult> ProcessPayment([FromBody] CreatePaymentDto paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Validation failed",
                    Errors = ModelState
                });
            }

            var result = await _paymentService.ProcessPaymentAsync(paymentDto);


            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Error
            });
        }

        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPaymentById(int paymentId)
        {
            var result = await _paymentService.GetPaymentByIdAsync(paymentId);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Data
            });
        }


        [HttpGet("DateRange")]
        public async Task<IActionResult> GetPaymentsByDateRange( [FromQuery] DateTime startDate,[FromQuery] DateTime endDate)
        {
            var result = await _paymentService.GetPaymentsByDateRangeAsync(startDate, endDate);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Data
            });
        }

        [HttpGet("Appointment/{appointmentId}/Total")]
        public async Task<IActionResult> GetTotalPaymentsForAppointment(int appointmentId)
        {
            var result = await _paymentService.GetTotalPaymentsForAppointmentAsync(appointmentId);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                TotalAmount = result.Data
            });
        }

        [HttpGet("Summary")]
        public async Task<IActionResult> GetPaymentSummary([FromQuery] DateTime? startDate = null,[FromQuery] DateTime? endDate = null)
        {
            var result = await _paymentService.GetPaymentSummaryAsync(startDate, endDate);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Data
            });
        }


        [HttpPost("{paymentId}/Refund")]
        public async Task<IActionResult> RefundPayment(int paymentId)
        {
            var result = await _paymentService.RefundPaymentAsync(paymentId);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Data
            });
        }


        [HttpGet("Appointment/{appointmentId}/PaymentStatus")]
        public async Task<IActionResult> CheckAppointmentPaymentStatus(int appointmentId)
        {
            var result = await _paymentService.CheckAppointmentPaymentStatusAsync(appointmentId);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                IsPaid = result.Data
            });
        }
        [HttpGet("MonthlySummary/{year}/{month}")]
        public async Task<IActionResult> GetMonthlyPaymentSummary(int year, int month)
        {
            var result = await _paymentService.GetMonthlyPaymentSummaryAsync(year, month);
            return StatusCode(result.StatusCode, new { result.Message, result.Error, result.Data });
        }
    }
}