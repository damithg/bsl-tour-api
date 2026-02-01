using System;
using BSLTours.API.Services;
using BSLTours.Communications.Core.Extensions;
using BSLTours.Communications.SendGrid.Extensions;
using BSLTours.Communications.Postmark.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<IStrapiService, StrapiService>();
builder.Services.AddHttpClient<ITourService, TourService>();
builder.Services.AddHttpClient<ITurnstileService, TurnstileService>();

// Configure email services with new Communications library
// Provider selection is driven by appsettings.json "EmailService:Provider"
var emailProvider = builder.Configuration["EmailService:Provider"]?.ToLower()
    ?? throw new InvalidOperationException("EmailService:Provider is not configured in appsettings.json");

switch (emailProvider)
{
    case "sendgrid":
        builder.Services.AddSendGridEmailProvider(options =>
        {
            options.ApiKey = Environment.GetEnvironmentVariable("SendGridApiKey")
                ?? throw new InvalidOperationException("SendGridApiKey environment variable is not set");
            options.DefaultFromEmail = builder.Configuration["SendGrid:DefaultFromEmail"];
            options.DefaultFromName = builder.Configuration["SendGrid:DefaultFromName"];
        });
        break;

    case "postmark":
        builder.Services.AddPostmarkEmailProvider(options =>
        {
            options.ServerToken = Environment.GetEnvironmentVariable("PostmarkServerToken")
                ?? throw new InvalidOperationException("PostmarkServerToken environment variable is not set");
            options.DefaultFromEmail = builder.Configuration["Postmark:DefaultFromEmail"];
            options.DefaultFromName = builder.Configuration["Postmark:DefaultFromName"];
        });
        break;

    default:
        throw new InvalidOperationException(
            $"Unknown email provider '{emailProvider}'. Supported providers: SendGrid, Postmark");
}

builder.Services.AddEmailService(builder.Configuration);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add API versioning - new in .NET 8
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "BSL Tours API", 
        Version = "v1",
        Description = "API for Best Sri Lanka Tours website"
    });
});


builder.Services.AddHttpClient<StrapiService>();

// Add observability - new in .NET 8
// Uncomment when adding OpenTelemetry NuGet packages
// builder.Services.AddOpenTelemetry()
//     .WithTracing(tracing => tracing
//         .AddAspNetCoreInstrumentation()
//         .AddHttpClientInstrumentation())
//     .WithMetrics(metrics => metrics
//         .AddAspNetCoreInstrumentation()
//         .AddHttpClientInstrumentation());


// Add AutoMapper using current assembly (or point to your profile assembly)
builder.Services.AddAutoMapper(typeof(Program));

// Force app to listen on port 80 for DigitalOcean
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// Disable HTTPS redirection for local development
app.UseHttpsRedirection();

// Use CORS middleware
app.UseCors();

// Add authorization middleware
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();
