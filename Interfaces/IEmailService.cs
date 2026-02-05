using commitment_calendar_api.Entities;

namespace commitment_calendar_api.Interfaces
{
    public interface IEmailService
    {
        Task ProcessEmails(CancellationToken cancellationToken);
        string CreateHtmlEmailBody(Appointment appointment);
    }
}
