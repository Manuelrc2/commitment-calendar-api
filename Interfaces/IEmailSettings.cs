namespace commitment_calendar_api.Interfaces
{
    public interface IEmailSettings
    {
        string SmtpServer
        {
            get; set;
        }
        int SmtpPort { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string FromName { get; set; }
    }
}
