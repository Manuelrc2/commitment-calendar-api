using commitment_calendar_api.Entities;

namespace commitment_calendar_api.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        int? ValidateToken(string token);
    }
}
