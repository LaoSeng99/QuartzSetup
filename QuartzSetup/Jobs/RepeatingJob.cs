using Quartz;

namespace QuartzSetup.Jobs;

public class RepeatingJob : IJob
{
    public static readonly JobKey RepeatingJobKey = new JobKey("repeatingJob", "backgroundGroup");
    public async Task Execute(IJobExecutionContext context)
    {
        var time = DateTime.Now;
        Console.WriteLine($"Repeating every 10 sec!");
        Console.WriteLine($"{time}");
    }
}