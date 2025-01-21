using Microsoft.AspNetCore.Mvc;
using Quartz;
using QuartzSetup.Jobs;
using QuartzSetup.Jobs.JobRequest;
using QuartzSetup.Services;
using System.Net;

namespace QuartzSetup.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IJobSchedulerService _scheduler;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IJobSchedulerService jobSchedulerService)
        {
            _logger = logger;
            _scheduler = jobSchedulerService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpPost("schedule-reminder")]
        public async Task<IActionResult> ScheduleReminderJob([FromBody] ReminderJobRequest request)
        {
            // 调度 ReminderJob
            await _scheduler.ScheduleJob(request);
            return Ok("Reminder job scheduled successfully!");
        }

        [HttpPost("schedule-notification")]
        public async Task<IActionResult> ScheduleNotificationJob([FromBody] NotificationJobRequest request)
        {
            // 调度 NotificationJob
            await _scheduler.ScheduleJob(request);
            return Ok("Notification job scheduled successfully!");
        }



    }
}

