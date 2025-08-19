using commitment_calendar_api.Dtos;

namespace commitment_calendar_api.Interfaces
{
    public interface IAppointmentService
    {
        MonthCalendar GetCalendarByUserAndDate(string userId, DateTime date);
    }
}
