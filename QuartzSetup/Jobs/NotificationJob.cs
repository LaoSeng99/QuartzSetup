using Quartz;

namespace QuartzSetup.Jobs;

public class NotificationJob : IJob
{
    public static readonly JobKey NotificationJobKey = new JobKey("notificationJob", "defaultGroup");
    public async Task Execute(IJobExecutionContext context)
    {
        var time = DateTime.Now;
        var message = context.MergedJobDataMap.GetString("Message");
        var recipient = context.MergedJobDataMap.GetString("Recipient");
        Console.WriteLine($"Message: {message}, Recipient : {recipient}");
        Console.WriteLine($"{time}");
    }
}
