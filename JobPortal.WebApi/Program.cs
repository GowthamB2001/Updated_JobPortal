using Microsoft.AspNetCore.Authentication.JwtBearer;
using JobPortal.WebApi.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;

using Serilog;
using JobPortal.Application.DependencyInjection;
using JobPortal.Infrastructure.DependencyInjection;
using JobPortal.Persistence.DependencyInjection;
using JobPortal.Infrastructure.Services;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddPersistenceServices(
    builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHttpClient<EmailService>();


var jwtSettings =
    builder.Configuration.GetSection("JwtSettings");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSettings["Key"]))
            };
    });
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("recruiter", new OpenApiInfo
    {
        Title = "JobPortal API - Recruiter",
        Version = "v1",
        Description = "Endpoints used by Recruiters (Auth, Company, Job Posting, Renewals, Interviews)"
    });

    c.SwaggerDoc("jobseeker", new OpenApiInfo
    {
        Title = "JobPortal API - Job Seeker",
        Version = "v1",
        Description = "Endpoints used by Job Seekers (Google Login, Profiles, Applications, Resumes, Job Search)"
    });

    c.SwaggerDoc("all", new OpenApiInfo
    {
        Title = "JobPortal API - All Endpoints",
        Version = "v1",
        Description = "All Portal Endpoints including Admin & Master Data"
    });

    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        if (docName == "all") return true;

        if (apiDesc.ActionDescriptor is ControllerActionDescriptor action)
        {
            var controller = action.ControllerName;
            var actionName = action.ActionName;
            var path = apiDesc.RelativePath?.ToLower() ?? "";

            if (docName == "recruiter")
            {
                if (controller is "Companies" or "JobRenewals" or "Recruiters") return true;
                if (controller == "Jobs" && !path.Contains("search")) return true;
                if (controller == "Auth" && actionName is "Login" or "Register" or "SendEmailVerification" or "VerifyEmail") return true;
                if (controller == "Interviews" && actionName is "Create" or "UpdateStatus" or "GetByApplication") return true;
                if (controller == "JobSeekerProfiles" && actionName is "GetAll") return true;
                if (controller == "JobSeekers" && actionName is "GetById") return true;
                if (controller == "JobSeekerSkills" && actionName is "GetSkills") return true;
                if (controller == "Skills") return true;
                return false;
            }

            if (docName == "jobseeker")
            {
                if (controller is "JobSeekerProfiles" or "Resumes" or "Applications") return true;
                if (controller == "Jobs" && path.Contains("search")) return true;
                if (controller == "Auth" && actionName is "GoogleLogin") return true;
                if (controller == "Interviews" && actionName is "Accept" or "Reject" or "GetByApplication") return true;
                if (controller == "JobSeekers" && actionName is "Create" or "GetById" or "Update") return true;
                if (controller is "JobSeekerSkills" or "Skills") return true;
                return false;
            }
        }

        return false;
    });

    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter JWT Token"
        });

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },
                Array.Empty<string>()
            }
        });
});

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/recruiter/swagger.json", "Recruiter API");
    c.SwaggerEndpoint("/swagger/jobseeker/swagger.json", "Job Seeker API");
    c.SwaggerEndpoint("/swagger/all/swagger.json", "All APIs");
    c.RoutePrefix = "swagger";
});
app.UseCustomExceptionMiddleware();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AngularPolicy");
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
