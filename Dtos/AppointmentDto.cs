namespace commitment_calendar_api.Dtos
{
    public class AppointmentDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Stake { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
    }
}
