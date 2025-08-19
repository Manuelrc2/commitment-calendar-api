using commitment_calendar_api.Dtos;
using commitment_calendar_api.Entities;
using commitment_calendar_api.Interfaces;
using commitment_calendar_api.Persistence;

namespace commitment_calendar_api.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AppointmentService(ApplicationDbContext applicationDbcontext)
        {
            _applicationDbContext = applicationDbcontext;
        }
        public MonthCalendar GetCalendarByUserAndDate(string userId, DateTime date)
        {
            Appointment[] appointments = _applicationDbContext.Appointments.Where(appointment => appointment.UserId == userId && appointment.StartsAt > date).OrderBy(appointment => appointment.StartsAt).ToArray();
            MonthCalendar calendar = GetEmptlyCalendar(date);
            Dictionary<int, List<AppointmentDto>> appoimentsGrouped = GroupAppointmentsByDate(appointments);
            if (appointments.Length == 0)
            {
                return calendar;
            }
            for (int i = 0; i < DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                if (appoimentsGrouped.Keys.Contains(i + 1))
                {
                    calendar.DaysSchedules[i].Appointments = appoimentsGrouped[i + 1].ToArray();
                }
            }
            return calendar;
        }
        private MonthCalendar GetEmptlyCalendar(DateTime date)
        {
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            MonthCalendar monthCalendar = new MonthCalendar()
            {
                Date = date,
                DaysSchedules = new DaySchedule[daysInMonth]
            };
            for (int i = 0; i < daysInMonth; i++)
            {
                DaySchedule daySchedule = new DaySchedule()
                {
                    Date = new DateTime(date.Year, date.Month, i + 1),
                    Appointments = new AppointmentDto[0]
                };
                monthCalendar.DaysSchedules[i] = daySchedule;
            }
            return monthCalendar;
        }
        private Dictionary<int, List<AppointmentDto>> GroupAppointmentsByDate(Appointment[] appointments)
        {
            Dictionary<int, List<AppointmentDto>> appoimentsGrouped = new Dictionary<int, List<AppointmentDto>> ();
            foreach (var appointment in appointments)
            {
                int day = appointment.StartsAt.Day;
                AppointmentDto appointmentDto = new AppointmentDto()
                {
                    Name = appointment.Name,
                    Description = appointment.Description,
                    Stake = appointment.Stake,
                    StartsAt = appointment.StartsAt,
                    EndsAt = appointment.EndsAt,
                };
                if (appoimentsGrouped.Keys.Contains(day))
                {
                    appoimentsGrouped[day].Add(appointmentDto);
                }
                else
                {
                    appoimentsGrouped[day] = new List<AppointmentDto>() { appointmentDto};
                }
            }
            return appoimentsGrouped;
        }
    }
}
