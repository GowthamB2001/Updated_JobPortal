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
using JobPortal.Persistence.Context;
using Microsoft.EntityFrameworkCore;
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
                if (controller is "Companies" or "JobRenewals" or "Recruiters" or "Notifications" or "AppInfo") return true;
                if (controller == "Applications" && actionName is "GetByJob" or "GetById" or "UpdateStatus") return true;
                if (controller == "Jobs" && !path.Contains("search")) return true;
                if (controller == "Auth" && actionName is "Login" or "Register" or "SendEmailVerification" or "VerifyEmail" or "ForgotPassword" or "VerifyResetOtp" or "ResetPassword") return true;
                if (controller == "Interviews" && actionName is "Create" or "UpdateStatus" or "GetByApplication") return true;
                if (controller == "JobSeekerProfiles" && actionName is "GetAll") return true;
                if (controller == "JobSeekers" && actionName is "GetById") return true;
                if (controller == "JobSeekerSkills" && actionName is "GetSkills") return true;
                if (controller == "Skills") return true;
                return false;
            }

            if (docName == "jobseeker")
            {
                if (controller is "JobSeekerProfiles" or "Resumes" or "Applications" or "Notifications" or "SavedJobs" or "AppInfo") return true;
                if (controller == "Dashboard" && actionName == "GetJobSeekerDashboard") return true;
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

try
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.ExecuteSqlRaw(@"
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Notifications')
        BEGIN
            CREATE TABLE Notifications (
                Id UNIQUEIDENTIFIER PRIMARY KEY,
                UserId UNIQUEIDENTIFIER NOT NULL,
                Title NVARCHAR(MAX) NOT NULL,
                Message NVARCHAR(MAX) NOT NULL,
                Type NVARCHAR(100) NOT NULL DEFAULT 'General',
                TargetUrl NVARCHAR(MAX) NULL,
                IsRead BIT NOT NULL DEFAULT 0,
                CreatedOn DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                ModifiedOn DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                IsDeleted BIT NOT NULL DEFAULT 0
            );
        END

        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SavedJobs')
        BEGIN
            CREATE TABLE SavedJobs (
                Id UNIQUEIDENTIFIER PRIMARY KEY,
                JobSeekerProfileId UNIQUEIDENTIFIER NOT NULL,
                JobId UNIQUEIDENTIFIER NOT NULL,
                CreatedOn DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                ModifiedOn DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                IsDeleted BIT NOT NULL DEFAULT 0
            );
        END

        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('JobSeekerProfiles') AND name = 'CurrentLocation')
        BEGIN
            ALTER TABLE JobSeekerProfiles ADD CurrentLocation NVARCHAR(MAX) NULL;
        END

        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('JobSeekerProfiles') AND name = 'EmploymentStatus')
        BEGIN
            ALTER TABLE JobSeekerProfiles ADD EmploymentStatus NVARCHAR(MAX) NULL;
        END

        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('JobSeekerProfiles') AND name = 'NoticePeriod')
        BEGIN
            ALTER TABLE JobSeekerProfiles ADD NoticePeriod INT NULL;
        END

        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('JobSeekerProfiles') AND name = 'TotalExperienceYears')
        BEGIN
            ALTER TABLE JobSeekerProfiles ADD TotalExperienceYears INT NULL;
        END

        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('JobSeekerProfiles') AND name = 'TotalExperienceMonths')
        BEGIN
            ALTER TABLE JobSeekerProfiles ADD TotalExperienceMonths INT NULL;
        END

        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('JobSeekerProfiles') AND name = 'TotalCompaniesWorked')
        BEGIN
            ALTER TABLE JobSeekerProfiles ADD TotalCompaniesWorked INT NULL;
        END

        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('JobSeekerProfiles') AND name = 'CompaniesWorkedJson')
        BEGIN
            ALTER TABLE JobSeekerProfiles ADD CompaniesWorkedJson NVARCHAR(MAX) NULL;
        END

        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('JobSeekerProfiles') AND name = 'LastCompanySalary')
        BEGIN
            ALTER TABLE JobSeekerProfiles ADD LastCompanySalary DECIMAL(18,2) NULL;
        END

        -- Recruiters Table Columns
        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Recruiters') AND name = 'FirstName')
        BEGIN
            ALTER TABLE Recruiters ADD FirstName NVARCHAR(MAX) NULL;
        END

        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Recruiters') AND name = 'LastName')
        BEGIN
            ALTER TABLE Recruiters ADD LastName NVARCHAR(MAX) NULL;
        END

        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Recruiters') AND name = 'Designation')
        BEGIN
            ALTER TABLE Recruiters ADD Designation NVARCHAR(MAX) NULL;
        END

        -- Jobs Table Columns
        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Jobs') AND name = 'AutoRepostDays')
        BEGIN
            ALTER TABLE Jobs ADD AutoRepostDays INT NOT NULL DEFAULT 1;
        END

        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Jobs') AND name = 'Currency')
        BEGIN
            ALTER TABLE Jobs ADD Currency NVARCHAR(MAX) NOT NULL DEFAULT 'INR';
        END

        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Jobs') AND name = 'IsDraft')
        BEGIN
            ALTER TABLE Jobs ADD IsDraft BIT NOT NULL DEFAULT 0;
        END

        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Jobs') AND name = 'MinSalary')
        BEGIN
            ALTER TABLE Jobs ADD MinSalary DECIMAL(18,2) NULL;
        END

        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Jobs') AND name = 'MaxSalary')
        BEGIN
            ALTER TABLE Jobs ADD MaxSalary DECIMAL(18,2) NULL;
        END

        -- Clean up legacy JobApplications foreign key and column if present
        IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_JobApplications_JobSeekers_JobSeekerId1')
        BEGIN
            ALTER TABLE JobApplications DROP CONSTRAINT FK_JobApplications_JobSeekers_JobSeekerId1;
        END

        IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('JobApplications') AND name = 'JobSeekerId1')
        BEGIN
            ALTER TABLE JobApplications DROP COLUMN JobSeekerId1;
        END
    ");
}
catch (Exception ex)
{
    Log.Warning(ex, "Failed to initialize or check database tables Notifications/SavedJobs.");
}

app.Run();
