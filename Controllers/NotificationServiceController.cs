using commitment_calendar_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace commitment_calendar_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationServiceController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationServiceController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("StartNotificationService")]
        public async Task<ActionResult> StartStartNotificationService(CancellationToken cancellationToken)
        {
            try
            {
                await _notificationService.StartAsync(cancellationToken);
                return Ok("Notification Service started successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error starting Notification Service: {ex.Message}");
            }
        }

        [HttpGet("StopNotificationService")]
        public async Task<ActionResult> StopNotificationService(CancellationToken cancellationToken)
        {
            try
            {
                await _notificationService.StopAsync(cancellationToken);
                return Ok("Notification Service stopped successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error stopping Notification Service: {ex.Message}");
            }
        }
    }
}
