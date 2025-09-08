using commitment_calendar_api.Dtos;
using commitment_calendar_api.Exceptions;
using commitment_calendar_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace commitment_calendar_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        [HttpGet("GetAppointmentsByUserAndDate")]
        public async Task<ActionResult> GetCalendarByUserAndDate(DateTime date, string timezone)
        {
            try
            {
                return Ok(await _appointmentService.GetCalendarByUserAndDate(User.FindFirstValue(ClaimTypes.NameIdentifier)!, date, timezone));
            }
            catch (Exception exception)
            {
                return StatusCode(500, new ProblemDetails()
                {
                    Type = "Internal Server Error",
                    Title = "Internal Server Error",
                    Detail = "An error occurred while extracting the calendar",
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }
        [HttpPost("CreateAppointment")]
        public async Task<ActionResult> CreateAppointment([FromBody]AppointmentDto appointmentDto)
        {
            try
            {
                await _appointmentService.CreateAppointment(User.FindFirstValue(ClaimTypes.NameIdentifier)!, appointmentDto);
                return Ok();
            }
            catch (Exceptions.ValidationException validationException)
            {
                return BadRequest(new ProblemDetails()
                {
                    Type = "Bad Request",
                    Title = "Validation failed",
                    Detail = validationException.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (Exception exception)
            {
                return StatusCode(500, new ProblemDetails()
                {
                    Type = "Internal Server Error",
                    Title = "Internal Server Error",
                    Detail = "An error occurred while saving the data",
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }
        [HttpDelete("DeleteAppointment")]
        public async Task<ActionResult> DeleteAppointment(long id)
        {
            try
            {
                await _appointmentService.DeleteAppointment(User.FindFirstValue(ClaimTypes.NameIdentifier)!, id);
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(new ProblemDetails()
                {
                    Type = "Bad Request",
                    Title = "Bad Request",
                    Detail = exception.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }
        [HttpPut("UpdateAppointment")]
        public async Task<ActionResult> UpdateAppointment([FromBody] AppointmentDto appointmentDto)
        {
            try
            {
                await _appointmentService.UpdateAppointment(User.FindFirstValue(ClaimTypes.NameIdentifier)!, appointmentDto);
                return Ok();
            }
            catch (Exceptions.ValidationException validationException)
            {
                return BadRequest(new ProblemDetails()
                {
                    Type = "Bad Request",
                    Title = "Validation failed",
                    Detail = validationException.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (Exception exception)
            {
                return StatusCode(500, new ProblemDetails()
                {
                    Type = "Internal Server Error",
                    Title = "Internal Server Error",
                    Detail = "An error occurred while saving the data",
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }
    }
}
