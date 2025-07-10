using AppointmentBooking.src.Application.Appointments;
using AppointmentBooking.src.Application.Appointments.Commands;
using AppointmentBooking.src.Application.Appointments.Commands.CreateAppointment;
using AppointmentBooking.src.Application.Common.Interfaces;
using AppointmentBooking.src.Application.Providers.Commands.CreateProvider;
using AppointmentBooking.src.Application.WorkingHoursF.Commands.CreateWorkingHours;
using AppointmentBooking.src.Infrastructure;
using AppointmentBooking.src.Infrastructure.BackgroundJobs;
using AppointmentBooking.src.Infrastructure.Email;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Appointment Booking API",
        Version = "v1",
        Description = "An API for booking appointments with service providers"
    });

    options.SchemaFilter<TimeOnlySchemaFilter>();
});

// App services
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddScoped<IAppointmentValidatorService, AppointmentValidatorService>();
builder.Services.AddScoped<AppointmentReminderService>();
builder.Services.AddHostedService<ReminderBackgroundService>();

// Validators
builder.Services.AddValidatorsFromAssemblyContaining<ProviderValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddWorkingHoursValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateAppointmentValidator>();
builder.Services.AddFluentValidationAutoValidation();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateProviderHandler).Assembly));

// App pipeline
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Appointment Booking API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
