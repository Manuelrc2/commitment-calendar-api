namespace commitment_calendar_api.Dtos
{
    public class DaySchedule
    {
        public DateTime Date { get; set; }
        public AppointmentDto[] Appointments { get; set; }
    }
}
