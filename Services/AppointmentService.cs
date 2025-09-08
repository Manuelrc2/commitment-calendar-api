using commitment_calendar_api.Dtos;
using commitment_calendar_api.Entities;
using commitment_calendar_api.Interfaces;
using commitment_calendar_api.Persistence;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace commitment_calendar_api.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AppointmentService(ApplicationDbContext applicationDbcontext)
        {
            _applicationDbContext = applicationDbcontext;
        }
        public async Task<MonthCalendar> GetCalendarByUserAndDate(string userId, DateTime date, string timezone)
        {
            Appointment[] appointments = await _applicationDbContext.Appointments.Where(appointment => appointment.UserId == userId && !appointment.IsDeleted && appointment.StartsAt.Month == date.Month).OrderBy(appointment => appointment.StartsAt).ToArrayAsync();
            ConvertDatesFromTimezone(appointments, timezone);
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
        public async Task CreateAppointment(string userId, AppointmentDto appointmentDto)
        {
            if (string.IsNullOrEmpty(appointmentDto.Name))
            {
                throw new ValidationException("Name has to be filled out");
            }
            if (appointmentDto.Stake == 0)
            {
                throw new ValidationException("Stake has to be filled out");
            }
            var appointment = new Appointment()
            {
                UserId = userId,
                Name = appointmentDto.Name,
                Description = appointmentDto.Description,
                Stake = appointmentDto.Stake,
                StartsAt = appointmentDto.StartsAt,
                EndsAt = appointmentDto.EndsAt,
            };
            _applicationDbContext.Appointments.Add(appointment);
            await _applicationDbContext.SaveChangesAsync();
        }
        public async Task DeleteAppointment(string userId, long id)
        {
            var appointment = await _applicationDbContext.Appointments
                .Where(appointment => appointment.AppointmentId == id && appointment.UserId == userId)
                .FirstOrDefaultAsync();
            if (appointment == null)
            {
                throw new Exception("No appointment was found with that id");
            }
            appointment.IsDeleted = true;
            await _applicationDbContext.SaveChangesAsync();
        }
        public async Task UpdateAppointment(string userId, AppointmentDto appointmentDto)
        {
            if (string.IsNullOrEmpty(appointmentDto.Name))
            {
                throw new ValidationException("Name has to be filled out");
            }
            if (appointmentDto.Stake == 0)
            {
                throw new ValidationException("Stake has to be filled out");
            }
            var appointment = await _applicationDbContext.Appointments
                .Where(appointment => appointment.AppointmentId == appointmentDto.Id && !appointment.IsDeleted && appointment.UserId == userId)
                .FirstOrDefaultAsync();
            if (appointment == null)
            {
                throw new ValidationException("No appointment was found with that id");
            }

            appointment.Name = appointmentDto.Name;
            appointment.Description = appointmentDto.Description;
            appointment.Stake = appointmentDto.Stake;
            appointment.StartsAt = appointmentDto.StartsAt;
            appointment.EndsAt = appointmentDto.EndsAt;

            await _applicationDbContext.SaveChangesAsync();
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
                    Id = appointment.AppointmentId,
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
        private void ConvertDatesFromTimezone(Appointment[] appointments, string timezone)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            foreach (var appointment in appointments)
            {
                appointment.StartsAt = TimeZoneInfo.ConvertTimeFromUtc(appointment.StartsAt, timeZoneInfo);
                appointment.EndsAt = TimeZoneInfo.ConvertTimeFromUtc(appointment.EndsAt, timeZoneInfo);
            }
        }
    }
}
