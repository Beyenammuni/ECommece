using ECommeceSystem.EF.Data;
using ECommeceSystem.EF.IRepositries;
using ECommeceSystem.EF.UnitOfWork;
using ECommerceSystem.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwagger();
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowAnyOrigin", builder =>
        builder.WithOrigins("http://localhost:5044")//that's the port of the frontend application
               .AllowAnyMethod()
               .AllowAnyHeader());
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("ECommerceSystem")));
builder.Services.AddAppServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerceSystem V1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();