using System.ComponentModel.DataAnnotations;

namespace commitment_calendar_api.Dtos
{
    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}
