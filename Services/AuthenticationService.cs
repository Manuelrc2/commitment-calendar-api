using commitment_calendar_api.Dtos;
using commitment_calendar_api.Entities;
using commitment_calendar_api.Interfaces;
using commitment_calendar_api.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace commitment_calendar_api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IJwtService _jwtService;

        public AuthenticationService(ApplicationDbContext applicationDbcontext, IJwtService jwtService)
        {
            _applicationDbContext = applicationDbcontext;
            _jwtService = jwtService;
        }
        public async Task<AuthResponse> RegisterAsync(Dtos.RegisterRequest request)
        {
            if (await _applicationDbContext.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new Exception("User with this email already exists");
            }

            var user = new User
            {
                Email = request.Email,
                Name = request.Name,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _applicationDbContext.Users.Add(user);
            await _applicationDbContext.SaveChangesAsync();

            var authResponse = new AuthResponse
            {
                Token = _jwtService.GenerateToken(user),
                User = new UserDto
                {
                    Email = user.Email,
                    Name = user.Name
                }
            };

            return authResponse;
        }
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Invalid email or password");
            }

            var token = _jwtService.GenerateToken(user);

            var response = new AuthResponse
            {
                Token = token,
                User = new UserDto
                {
                    Email = user.Email,
                    Name = user.Name
                }
            };

            return response;
        }
        public async Task<UserDto> VerifyTokenAsync(HttpRequest request)
        {
            var authHeader = request.Headers["Authorization"].FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
            {
                throw new Exception("Bad request");
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var userId = _jwtService.ValidateToken(token);

            if (userId == null)
            {
                throw new Exception("Couldn't validate token");
            }

            var user = await _applicationDbContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("Couldn't find user");
            }

            return new UserDto
            {
                Email = user.Email,
                Name = user.Name
            };
        }
    }
}
