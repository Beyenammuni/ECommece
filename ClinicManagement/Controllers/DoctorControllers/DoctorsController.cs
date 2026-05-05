using Microsoft.AspNetCore.Mvc;
using ClinicAppointmentHR.Dtos.DoctorDto;
using ClinicAppointment.Services.Interfaces;
using ClinicManagementSystem.App.Enums;

namespace ClinicAppointment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }


        [HttpGet("All")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _doctorService.GetAllDoctorsAsync();
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Data
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _doctorService.GetDoctorByIdAsync(id);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Error,
                result.Data
            });
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] DoctorDto doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Validation failed",
                    Errors = ModelState
                });
            }

            var result = await _doctorService.CreateDoctorAsync(doctor);

            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Error
            });
        }


        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] DoctorDto doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Validation failed",
                    Errors = ModelState
                });
            }

            var result = await _doctorService.UpdateDoctorAsync(id, doctor);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Error,
                result.Data
            });
        }


        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _doctorService.DeleteDoctorAsync(id);

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

        [HttpGet("ByDepartment/{departmentId}")]
        public async Task<IActionResult> GetByDepartmentAsync(int departmentId)
        {
            var result = await _doctorService.GetDoctorsByDepartmentAsync(departmentId);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Data
            });
        }

        [HttpGet("BySpecialty/{specialty}")]
        public async Task<IActionResult> GetBySpecialtyAsync(SpecialityEnum specialty)
        {
            var result = await _doctorService.GetDoctorsBySpecialtyAsync(specialty);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Data
            });
        }
        [HttpPost("{doctorId}/UploadImage")]
        public async Task<IActionResult> UploadImage(int doctorId, IFormFile file)
        {
            var result = await _doctorService.UploadProfileImageAsync(doctorId, file);
            return StatusCode(result.StatusCode, new { result.Message, result.Error, Data = result.Data });
        }

        [HttpGet("{doctorId}/Image")]
        public async Task<IActionResult> GetImageUrl(int doctorId)
        {
            var result = await _doctorService.GetProfileImageUrlAsync(doctorId);
            return StatusCode(result.StatusCode, new { result.Message, result.Error, Data = result.Data });
        }

        [HttpDelete("{doctorId}/Image")]
        public async Task<IActionResult> DeleteImage(int doctorId)
        {
            var result = await _doctorService.DeleteProfileImageAsync(doctorId);
            return StatusCode(result.StatusCode, new { result.Message, result.Error });
        }
    }
}