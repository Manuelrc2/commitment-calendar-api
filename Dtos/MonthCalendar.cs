namespace commitment_calendar_api.Dtos
{
    public class MonthCalendar
    {
        public DateTime Date { get; set; }
        public DaySchedule[] DaysSchedules { get; set; }
    }
}
