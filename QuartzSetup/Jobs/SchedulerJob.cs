using Microsoft.EntityFrameworkCore;
using Quartz;
using QuartzSetup.Data;
namespace QuartzSetup.Jobs;

public class SchedulerJob(
    AppDbContext dbContext
    ) : IJob
{
    public static readonly JobKey SchedulerJobKey = new JobKey("schedulerJob", "backgroundGroup");
    public async Task Execute(IJobExecutionContext context)
    {
        var time = DateTime.Now;
        Console.WriteLine($"Scheduler Db Task at 1:30 PM!");
        Console.WriteLine($"Current: {time}");

        var users = await dbContext.Users.ToListAsync();
        Console.WriteLine($"Total Users: {users.Count}");

    }
}
