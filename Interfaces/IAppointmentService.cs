using commitment_calendar_api.Dtos;

namespace commitment_calendar_api.Interfaces
{
    public interface IAppointmentService
    {
        Task<MonthCalendar> GetCalendarByUserAndDate(long userId, DateTime date, string timezone);
        Task CreateAppointment(long userId, AppointmentDto appointmentDto);
        Task DeleteAppointment(long userId, long id);
        Task UpdateAppointment(long userId, AppointmentDto appointmentDto);
    }
}
