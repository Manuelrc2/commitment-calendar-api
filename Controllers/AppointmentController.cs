using commitment_calendar_api.Dtos;
using commitment_calendar_api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace commitment_calendar_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        [HttpGet("GetAppointmentsByUserAndDate")]
        public MonthCalendar GetCalendarByUserAndDate(string userId, DateTime date)
        {
            return _appointmentService.GetCalendarByUserAndDate(userId, date);
        }
    }
}
