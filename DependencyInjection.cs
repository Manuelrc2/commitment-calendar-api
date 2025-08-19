using commitment_calendar_api.Persistence;
using Microsoft.EntityFrameworkCore;
using commitment_calendar_api.Interfaces;
using commitment_calendar_api.Services;

namespace commitment_calendar_api
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            return services;
        }

    }
}