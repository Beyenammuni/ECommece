using ClinicAppointmentHR.Dtos.PatientDtos;
using ClinicAppointmentHR.Models;
using ClinicManagement.Main.Results;
using ClinicManagementSystem.App.Dtos.PatientDtos;

public interface IPatientService
{
    Task<ServiceResult<List<PatientModel>>> GetAllPatientsAsync();
    Task<ServiceResult<PatientModel>> GetPatientByIdAsync(int id);
    Task<ServiceResult<PatientModel>> CreatePatientAsync(PatientDto patientDto);
    Task<ServiceResult<PatientModel>> UpdatePatientAsync(int id, PatientDto patientDto);
    Task<ServiceResult<bool>> DeletePatientAsync(int id);
    Task<ServiceResult<List<PatientModel>>> SearchPatientsAsync(SearchPatient searchCriteria);
    Task<ServiceResult<List<AppointmentModel>>> GetPatientAppointmentsAsync(int patientId);
    Task<ServiceResult<bool>> ValidatePatientEmailAsync(string email, int? excludePatientId = null);
}