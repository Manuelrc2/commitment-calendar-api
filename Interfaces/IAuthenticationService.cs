using commitment_calendar_api.Dtos;

namespace commitment_calendar_api.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<UserDto?> VerifyTokenAsync(HttpRequest httpRequest);
    }
}
