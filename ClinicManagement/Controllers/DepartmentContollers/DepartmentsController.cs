using Microsoft.AspNetCore.Mvc;
using ClinicAppointmentHR.Dtos.DepartmentDto;
using ClinicAppointment.Services.Interfaces;

namespace ClinicAppointment.Controllers.DepartmentContollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _departmentService.GetAllDepartmentsAsync();
            return StatusCode(
                result.StatusCode, new 
                {
                    result.Message,
                    result.Data
                }
                );
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] DepartmentDto department)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _departmentService.CreateDepartmentAsync(department);

            if (result.StatusCode == 201)
            {
                return Ok(result);
            }

            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Error
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _departmentService.GetDepartmentByIdAsync(id);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Error,
                result.Data
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] DepartmentDto department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _departmentService.UpdateDepartmentAsync(id, department);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Data
            });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _departmentService.DeleteDepartmentAsync(id);

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

        [HttpGet("GetDoctorByDepartmentId/{departmentId}")]
        public async Task<IActionResult> GetDoctorsByDepartmentIdAsync(int departmentId)
        {
            var result = await _departmentService.GetDoctorsByDepartmentIdAsync(departmentId);
            return StatusCode(result.StatusCode, new
            {
                result.Message,
                result.Data
            });
        }
    }
}