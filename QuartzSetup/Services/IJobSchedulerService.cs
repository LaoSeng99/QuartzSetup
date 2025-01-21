using Quartz;
using QuartzSetup.Jobs.JobRequest;

namespace QuartzSetup.Services;

public interface IJobSchedulerService
{
    Task ClearAllJobs();
    Task ScheduleBackgroundJob();
    Task ScheduleJob(IJobRequest request);
}
