using commitment_calendar_api.Dtos;
using commitment_calendar_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public MonthCalendar GetCalendarByUserAndDate(DateTime date)
        {
            return _appointmentService.GetCalendarByUserAndDate(User.FindFirstValue(ClaimTypes.NameIdentifier)!, date);
        }
    }
}
