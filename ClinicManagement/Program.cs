using ClinicAppointment.Services.Implementations;
using ClinicAppointment.Services.Interfaces;
using ClinicAppointmentHR.Data;
using ClinicAppointmentHR.Helper;
using ClinicAppointmentHR.IServices;
using ClinicAppointmentHR.Services;
using ClinicManagement.App.Models;
using ClinicManagement.Helper;
using ClinicManagement.Main.IServices;
using ClinicManagement.Main.Services;
using ClinicManagement.Main.Services.ClinicAppointment.Services.Implementations;
using ClinicManagementSystem.App.Services.Implementations;
using ClinicManagementSystem.App.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthentication(builder.Configuration);
var options = new JsonSerializerOptions
{
    ReferenceHandler = ReferenceHandler.Preserve,
    WriteIndented = true
};

var json = JsonSerializer.Serialize("ClinicManagement", options);


builder.Services.AddSwaggerConfiguration();
builder.Services.AddControllers().AddJsonOptions(options=>options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddCors(
    options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });
builder.Services.AddScoped<ITokenService, ServiceToken>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDoctorWorkingHoursService, DoctorWorkingHoursService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddIdentity<UserModel, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("ClinicManagement")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
   app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "ClinicManagement");
        options.RoutePrefix = "swagger";
    }); 
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.UseCors("AllowAll");    

app.Run();
