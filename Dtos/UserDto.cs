using System.ComponentModel.DataAnnotations;

namespace commitment_calendar_api.Dtos
{
    public class UserDto
    {
        public string Email { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

    }
}
