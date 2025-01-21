using Quartz;

namespace QuartzSetup.Jobs;

public class ReminderJob : IJob
{
    public static readonly JobKey ReminderJobKey = new JobKey("reminderJob", "defaultGroup");
    public async Task Execute(IJobExecutionContext context)
    {
        var reminderMessage = context.JobDetail.JobDataMap.GetString("Message");

        var time = DateTime.Now;
        Console.WriteLine($"Remind User Message: {reminderMessage} ");
        Console.WriteLine($"{time}");

    }
}
