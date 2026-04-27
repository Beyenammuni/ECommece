
using ClinicAppointmentHR.Dtos.AppointmentDtos;
using ClinicManagement.App.Dtos.BonusDtos;
using ClinicManagementSystem.App.Dtos.AppointmentDtos;
using ClinicManagementSystem.App.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.Controllers.AppointmentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("Schedule")]
        public async Task<IActionResult> ScheduleAppointment([FromBody] ScheduletAppointmentDto schedule)
        {
            var result = await _appointmentService.ScheduleAppointmentAsync(schedule);
            return StatusCode(result.StatusCode, new { result.Message, result.Data });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] UpdateAppoitmentDto update)
        {
            var result = await _appointmentService.UpdateAppointmentAsync(id, update);
            return StatusCode(result.StatusCode, new { result.Message, result.Data });
        }

        [HttpPut("Cancel/{id}")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var result = await _appointmentService.CancelAppointmentAsync(id);
            return StatusCode(result.StatusCode, new { result.Message, result.Data });
        }

        [HttpGet("Completed")]
        public async Task<IActionResult> GetCompletedAppointments()
        {
            var result = await _appointmentService.GetCompletedAppointmentsAsync();
            return StatusCode(result.StatusCode, new { result.Message, result.Data });
        }

        [HttpPost("GetByDateAndDoctor")]
        public async Task<IActionResult> GetAppointmentsByDateAndDoctor([FromBody] getAppointmentByDotorAndDateDto appointment)
        {
            var result = await _appointmentService.GetAppointmentsByDateAndDoctorAsync(appointment);
            return StatusCode(result.StatusCode, new { result.Message, result.Data });
        }

        [HttpGet("Today")]
        public async Task<IActionResult> GetTodayAppointments()
        {
            var result = await _appointmentService.GetTodayAppointmentsAsync();
            return StatusCode(result.StatusCode, new { result.Message,  result.Data });
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var result = await _appointmentService.GetAllAppointmentsAsync();
            return StatusCode(result.StatusCode, new { result.Message,  result.Data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var result = await _appointmentService.GetAppointmentByIdAsync(id);
            return StatusCode(result.StatusCode, new { result.Message,  result.Data });
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentDto appointment)
        {
            var result = await _appointmentService.CreateAppointmentAsync(appointment);
            return StatusCode(result.StatusCode, new { result.Message, result.Data });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var result = await _appointmentService.DeleteAppointmentAsync(id);
            if (result.StatusCode == 204)
                return NoContent();
            return StatusCode(result.StatusCode, new { result.Message, result.Error });
        }
        [HttpPost("Filter")]
        public async Task<IActionResult> FilterAppointments([FromBody] AppointmentFilterDto filter)
        {
            var result = await _appointmentService.GetFilteredAppointmentsAsync(filter);
            return StatusCode(result.StatusCode, new { result.Message, result.Data });
        }
    }
}