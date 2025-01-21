namespace QuartzSetup.Jobs.JobRequest;

public class ReminderJobRequest : IJobRequest
{
    public string Message { get; set; }
    public DateTime ReminderTime { get; set; }
}
