using Quartz;
using Quartz.Impl.Matchers;
using QuartzSetup.Factories;
using QuartzSetup.Jobs;
using QuartzSetup.Jobs.JobRequest;


namespace QuartzSetup.Services;

public class JobSchedulerService(
    IScheduler scheduler,
    IServiceProvider serviceProvider
    ) : IJobSchedulerService
{
    public async Task ScheduleJob(IJobRequest request)
    {
        IJobFactory jobFactory = request switch
        {
            ReminderJobRequest _ => serviceProvider.GetRequiredService<ReminderJobFactory>(),
            NotificationJobRequest _ => serviceProvider.GetRequiredService<NotificationJobFactory>(),
            _ => throw new NotImplementedException("Job type not supported.")
        };

        var job = jobFactory.CreateJob(request);
        var trigger = jobFactory.CreateTrigger(request);

        await scheduler.ScheduleJob(job, trigger);
    }

    public async Task ScheduleBackgroundJob()
    {
        Dictionary<JobKey, IJobFactory> jobFactories = new Dictionary<JobKey, IJobFactory>
        {
            {RepeatingJob.RepeatingJobKey, serviceProvider.GetRequiredService<RepeatingJobFactory>()},
            {SchedulerJob.SchedulerJobKey, serviceProvider.GetRequiredService<SchedulerJobFactory>()},
            // 如果你有其他作业，可以类似地添加到字典中
        };
        foreach (var jobFactory in jobFactories)
        {
            var jobKey = jobFactory.Key;
            var factory = jobFactory.Value;

            var jobDetail = await scheduler.GetJobDetail(jobKey);
            if (jobDetail == null)
            {
                // 创建 JobDetail 和 Trigger
                var job = factory.CreateJob();
                var trigger = factory.CreateTrigger();
                // 调度作业
                await scheduler.ScheduleJob(job, trigger);
            }
        }
    }

    public async Task ClearAllJobs()
    {
        await scheduler.Clear();
        Console.WriteLine("All jobs cleared.");
    }

    public async Task ClearJobsByGroup(string groupName)
    {
        var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName));

        foreach (var jobKey in jobKeys)
        {
            await scheduler.DeleteJob(jobKey);
            Console.WriteLine($"Job {jobKey} in group {groupName} deleted.");
        }
    }

    public async Task ClearTriggersByGroup(string groupName)
    {
        var triggerKeys = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals(groupName));

        foreach (var triggerKey in triggerKeys)
        {
            await scheduler.UnscheduleJob(triggerKey);
            Console.WriteLine($"Trigger {triggerKey} in group {groupName} unscheduled.");
        }
    }
}
