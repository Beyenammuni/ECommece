using ClinicManagement.App.Dtos.RegistrationDtos.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicAppointmentHR.IServices
{
    public interface ITokenService
    {
        string GenerateToken(RegisterDto username);
    }
}
