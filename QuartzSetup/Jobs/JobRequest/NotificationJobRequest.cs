namespace QuartzSetup.Jobs.JobRequest;

public class NotificationJobRequest : IJobRequest
{
    public string Message { get; set; }
    public DateTime ReminderTime { get; set; }
    public string Recipient { get; set; }
}