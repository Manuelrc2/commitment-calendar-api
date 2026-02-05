using commitment_calendar_api.Interfaces;

namespace commitment_calendar_api.Dtos.Settings
{
    public class EmailSettings : IEmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
    }
}
