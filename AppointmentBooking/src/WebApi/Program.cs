using AppointmentBooking.src.Application.Appointments;
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
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

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


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<ProviderValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateProviderHandler).Assembly));
builder.Services.AddValidatorsFromAssemblyContaining<AddWorkingHoursValidator>();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddScoped<AppointmentReminderService>();
builder.Services.AddHostedService<ReminderBackgroundService>();



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
