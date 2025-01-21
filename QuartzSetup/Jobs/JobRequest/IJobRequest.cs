namespace QuartzSetup.Jobs.JobRequest;

public interface IJobRequest
{
    string Message { get; }
    DateTime ReminderTime { get; }
}
