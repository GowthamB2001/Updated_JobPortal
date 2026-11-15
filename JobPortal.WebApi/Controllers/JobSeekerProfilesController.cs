using JobPortal.Application.Features.JobSeekers.Commands.RegisterJobSeeker;
using Microsoft.AspNetCore.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using JobPortal.Application.Features.RegisterJobSeeker.Command.UpdateJobSeekerProfile;
using JobPortal.Application.Features.RegisterJobSeeker.Queries.GetAllJobSeekerProfiles;
using JobPortal.Application.Features.RegisterJobSeeker.Queries.GetJobSeekerProfileById;
using JobPortal.Application.Features.RegisterJobSeeker.Command.DeleteJobSeekerProfile;
using JobPortal.Application.Features.RegisterJobSeeker.Queries.LoginJobSeeker;
using JobPortal.Application.Features.Jobs.Queries.SearchJobs;
using JobPortal.Application.Features.Jobs.Queries.GetAllJobs;
using JobPortal.Application.Features.Dashboard.Queries.GetJobSeekerDashboard;
using JobPortal.Application.DTOs.SavedJobs;
using JobPortal.Application.Features.SavedJobs.Commands.DeleteSavedJob;
using JobPortal.Application.Features.SavedJobs.Commands.SaveJob;
using JobPortal.Application.Features.SavedJobs.Queries.GetSavedJobs;
using JobPortal.Application.Features.Notifications.Commands.MarkAllNotificationsAsRead;
using JobPortal.Application.Features.Notifications.Commands.MarkNotificationAsRead;
using JobPortal.Application.Features.Notifications.Queries.GetNotificationCount;
using JobPortal.Application.Features.Notifications.Queries.GetNotifications;
using JobPortal.Application.Features.AppInfo.Queries.GetAppInfo;
using JobPortal.Application.Features.AppInfo.Queries.GetPrivacyPolicy;
using JobPortal.Application.Features.AppInfo.Queries.GetTermsAndConditions;
using JobPortal.Application.Features.JobApplications.Queries.GetApplicationsByJobSeeker;

namespace JobPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSeekerProfilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobSeekerProfilesController(IMediator mediator) { 
        
            _mediator = mediator;
        
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(
                new GetAllJobSeekerProfilesQuery());

            return Ok(result);
        }

        [Authorize]
        [HttpGet("my-profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var profileId))
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = "Invalid or missing token claims."
                });
            }

            var result = await _mediator.Send(
                new GetJobSeekerProfileByIdQuery
                {
                    Id = profileId
                });

            if (result == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Job seeker profile not found."
                });
            }

            return Ok(result);
        }
        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Register(
     [FromForm] RegisterJobSeekerCommand command)
        {
            var id = await _mediator.Send(command);

            return Ok(new
            {
                Success = true,
                Message = "Job seeker profile completed successfully.",
                JobSeekerId = id,
                IsProfileCompleted = true
            });
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(
    Guid id,
    [FromForm] UpdateJobSeekerProfileCommand command)
        {
            command.Id = id;

            await _mediator.Send(command);

            return Ok(new
            {
                Success = true,
                Message = "Profile updated successfully."
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(
                new DeleteJobSeekerProfileCommand
                {
                    Id = id
                });

            return Ok(new
            {
                Success = true,
                Message = "Account deleted successfully."
            });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] JobSeekerLoginQuery query)
        {
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Mobile number not registered."
                });
            }

            return Ok(new
            {
                Success = true,
                User = result
            });
        }
        [HttpGet("JobsList")]
        public async Task<IActionResult> GetAllJobs()
        {
            var result = await _mediator.Send(
                new SearchJobsQuery
                {
                    PageNumber = 1,
                    PageSize = 100
                });

            return Ok(result);
        }
        [HttpGet("Fulljoblist")]
        public async Task<IActionResult> GetAllJobList()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;
            Guid? seekerId = Guid.TryParse(userIdClaim, out var id) ? id : null;

            var result = await _mediator.Send(new GetAllJobsQuery { JobSeekerId = seekerId });

            return Ok(new
            {
                Success = true,
                Jobs = result
            });
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;
            Guid? seekerId = Guid.TryParse(userIdClaim, out var id) ? id : null;

            var result = await _mediator.Send(new GetJobSeekerDashboardQuery { JobSeekerId = seekerId });

            return Ok(new
            {
                Success = true,
                Data = result
            });
        }

        // ======================= Applied Jobs =======================
        [HttpGet("applied-jobs")]
        public async Task<IActionResult> GetAppliedJobs([FromQuery] string? status = null)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var seekerId))
                return Unauthorized(new { Success = false, Message = "Unauthorized: Invalid or missing token claims." });

            var result = await _mediator.Send(new GetApplicationsByJobSeekerQuery
            {
                JobSeekerId = seekerId,
                Status = status
            });

            return Ok(new
            {
                Success = true,
                TotalCount = result.Count,
                Data = result
            });
        }

        // ======================= Saved Jobs =======================
        [HttpGet("saved-jobs")]
        public async Task<IActionResult> GetSavedJobs()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var seekerId))
                return Unauthorized(new { Success = false, Message = "Unauthorized: Invalid or missing token claims." });

            var result = await _mediator.Send(new GetSavedJobsQuery { JobSeekerProfileId = seekerId });

            return Ok(new
            {
                Success = true,
                TotalCount = result.Count,
                Data = result
            });
        }

        [HttpPost("saved-jobs")]
        public async Task<IActionResult> SaveJob([FromBody] SaveJobRequestDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var seekerId))
                return Unauthorized(new { Success = false, Message = "Unauthorized: Invalid or missing token claims." });

            if (request == null || request.JobId == Guid.Empty)
                return BadRequest(new { Success = false, Message = "JobId is required." });

            try
            {
                var result = await _mediator.Send(new SaveJobCommand
                {
                    JobSeekerProfileId = seekerId,
                    JobId = request.JobId
                });

                return Ok(new
                {
                    Success = true,
                    Message = "Job saved successfully.",
                    Data = result
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Success = false, Message = ex.Message });
            }
        }

        [HttpDelete("saved-jobs/{jobId}")]
        public async Task<IActionResult> DeleteSavedJob(Guid jobId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var seekerId))
                return Unauthorized(new { Success = false, Message = "Unauthorized: Invalid or missing token claims." });

            var success = await _mediator.Send(new DeleteSavedJobCommand
            {
                JobSeekerProfileId = seekerId,
                JobId = jobId
            });

            if (!success)
                return NotFound(new { Success = false, Message = "Saved job not found or already removed." });

            return Ok(new
            {
                Success = true,
                Message = "Job removed from saved jobs successfully."
            });
        }

        // ======================= Notifications =======================
        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var seekerId))
                return Unauthorized(new { Success = false, Message = "Unauthorized: Invalid or missing token claims." });

            var result = await _mediator.Send(new GetNotificationsQuery { UserId = seekerId });

            return Ok(result);
        }

        [HttpGet("notifications/count")]
        public async Task<IActionResult> GetNotificationCount()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var seekerId))
                return Unauthorized(new { Success = false, Message = "Unauthorized: Invalid or missing token claims." });

            var result = await _mediator.Send(new GetNotificationCountQuery { UserId = seekerId });

            return Ok(result);
        }

        [HttpPut("notifications/{id}/read")]
        public async Task<IActionResult> MarkNotificationAsRead(Guid id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var seekerId))
                return Unauthorized(new { Success = false, Message = "Unauthorized: Invalid or missing token claims." });

            await _mediator.Send(new MarkNotificationAsReadCommand
            {
                NotificationId = id,
                UserId = seekerId
            });

            return Ok(new
            {
                Success = true,
                Message = "Notification marked as read."
            });
        }

        [HttpPut("notifications/read-all")]
        public async Task<IActionResult> MarkAllNotificationsAsRead()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var seekerId))
                return Unauthorized(new { Success = false, Message = "Unauthorized: Invalid or missing token claims." });

            await _mediator.Send(new MarkAllNotificationsAsReadCommand
            {
                UserId = seekerId
            });

            return Ok(new
            {
                Success = true,
                Message = "All notifications marked as read."
            });
        }

        // ======================= App Info & Legal =======================
        [AllowAnonymous]
        [HttpGet("about")]
        public async Task<IActionResult> GetAboutApp()
        {
            var result = await _mediator.Send(new GetAppInfoQuery());
            return Ok(new
            {
                Success = true,
                Data = result
            });
        }

        [AllowAnonymous]
        [HttpGet("privacy-policy")]
        public async Task<IActionResult> GetPrivacyPolicy()
        {
            var result = await _mediator.Send(new GetPrivacyPolicyQuery());
            return Ok(new
            {
                Success = true,
                Data = result
            });
        }

        [AllowAnonymous]
        [HttpGet("terms-and-conditions")]
        public async Task<IActionResult> GetTermsAndConditions()
        {
            var result = await _mediator.Send(new GetTermsAndConditionsQuery());
            return Ok(new
            {
                Success = true,
                Data = result
            });
        }
    }
}
