using Microsoft.AspNetCore.Mvc;
using ClinicAppointmentHR.Dtos.PatientDtos;
using ClinicManagementSystem.App.Dtos.PatientDtos;
using ClinicAppointment.Services.Interfaces;

namespace ClinicManagementSystem.Controllers.PatientControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _patientService.GetAllPatientsAsync();
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Data
            });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _patientService.GetPatientByIdAsync(id);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Data
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] PatientDto patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Validation failed",
                    Errors = ModelState
                });
            }

            var result = await _patientService.CreatePatientAsync(patient);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Error
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] PatientDto patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Validation failed",
                    Errors = ModelState
                });
            }

            var result = await _patientService.UpdatePatientAsync(id, patient);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Data
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _patientService.DeletePatientAsync(id);

            if (result.StatusCode == 204)
            {
                return NoContent();
            }

            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Error
            });
        }

   
        [HttpPost("Search")]
        public async Task<IActionResult> SearchPatients([FromBody] SearchPatient searchCriteria)
        {
            var result = await _patientService.SearchPatientsAsync(searchCriteria);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Data
            });
        }


        [HttpGet("{patientId}/Appointments")]
        public async Task<IActionResult> GetPatientAppointments(int patientId)
        {
            var result = await _patientService.GetPatientAppointmentsAsync(patientId);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Error,
            });
        }

        [HttpGet("ValidateEmail")]
        public async Task<IActionResult> ValidateEmail([FromQuery] string email, [FromQuery] int? excludePatientId = null)
        {
            var result = await _patientService.ValidatePatientEmailAsync(email, excludePatientId);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Error,
                IsAvailable = result.IsSuccess
            });
        }
    }
}