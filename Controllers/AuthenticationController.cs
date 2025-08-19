using commitment_calendar_api.Dtos;
using commitment_calendar_api.Entities;
using commitment_calendar_api.Interfaces;
using commitment_calendar_api.Persistence;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace commitment_calendar_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthenticationService _authenticationService;
        public HttpRequest Request => HttpContext?.Request!;

        public AuthenticationController(ApplicationDbContext context, IAuthenticationService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<AuthResponse>> Register(Dtos.RegisterRequest request)
        {
            try
            {
                return Ok(await _authenticationService.RegisterAsync(request));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponse>> Login(Dtos.LoginRequest request)
        {
            try
            {
                return Ok(await _authenticationService.LoginAsync(request));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("verify")]
        public async Task<ActionResult<UserDto>> VerifyToken()
        {
            try
            {
                return Ok(await _authenticationService.VerifyTokenAsync(Request));
            }
            catch (Exception exception)
            {
                return Unauthorized(exception.Message);
            }
        }
    }
}
