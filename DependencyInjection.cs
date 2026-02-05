using commitment_calendar_api.Persistence;
using Microsoft.EntityFrameworkCore;
using commitment_calendar_api.Interfaces;
using commitment_calendar_api.Services;
using commitment_calendar_api.Dtos.Settings;

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
            services.AddSingleton<IEmailSettings, EmailSettings>(settings => new EmailSettings
            {
                SmtpServer = configuration.GetValue<string>("EmailSettings:SmtpServer"),
                SmtpPort = configuration.GetValue<int>("EmailSettings:SmtpPort"),
                Username = configuration.GetValue<string>("EmailSettings:Username"),
                Password = configuration.GetValue<string>("EmailSettings:Password"),
                FromName = configuration.GetValue<string>("EmailSettings:FromName")
            });
            services.AddScoped<IEmailService, EmailService>();
            services.AddSingleton<NotificationService, NotificationService>()
                .AddHostedService(services => services.GetRequiredService<NotificationService>());
            return services;
        }

    }
}