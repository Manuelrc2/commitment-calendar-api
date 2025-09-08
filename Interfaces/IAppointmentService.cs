using commitment_calendar_api.Dtos;

namespace commitment_calendar_api.Interfaces
{
    public interface IAppointmentService
    {
        Task<MonthCalendar> GetCalendarByUserAndDate(string userId, DateTime date, string timezone);
        Task CreateAppointment(string userId, AppointmentDto appointmentDto);
        Task DeleteAppointment(string userId, long id);
        Task UpdateAppointment(string userId, AppointmentDto appointmentDto);
    }
}
