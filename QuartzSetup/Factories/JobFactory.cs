using Azure.Core;
using Quartz;
using QuartzSetup.Jobs;
using QuartzSetup.Jobs.JobRequest;

namespace QuartzSetup.Factories;
public class ReminderJobFactory : BaseJobFactory
{
    public override IJobDetail CreateJob(IJobRequest request)
    {
        var req = request as ReminderJobRequest;

        return JobBuilder.Create<ReminderJob>()
            .WithIdentity(ReminderJob.ReminderJobKey)  // 使用 JobKey
            .UsingJobData("Message", req!.Message)
            .Build();
    }

    public override ITrigger CreateTrigger(IJobRequest request)
    {
        return TriggerBuilder.Create()
            .WithIdentity($"trigger_{Guid.NewGuid()}", "defaultGroup")
            .StartAt(request.ReminderTime)
            .ForJob(ReminderJob.ReminderJobKey) // 绑定触发器到特定的 Job
            .Build();
    }
}


public class NotificationJobFactory : BaseJobFactory
{
    public override IJobDetail CreateJob(IJobRequest request)
    {
        var req = request as NotificationJobRequest;
        return JobBuilder.Create<NotificationJob>()
            .WithIdentity(NotificationJob.NotificationJobKey) // 使用 JobKey
            .UsingJobData("Message", req!.Message)
            .UsingJobData("Recipient", req.Recipient)
            .Build();
    }

    public override ITrigger CreateTrigger(IJobRequest request)
    {
        return TriggerBuilder.Create()
            .WithIdentity($"trigger_{Guid.NewGuid()}", "defaultGroup")
            .ForJob(NotificationJob.NotificationJobKey) // 绑定触发器到特定的 Job
            .StartAt(request.ReminderTime)
            .Build();
    }
}

public class RepeatingJobFactory : BaseJobFactory
{
    public override IJobDetail CreateJob()
    {
        return JobBuilder.Create<RepeatingJob>()
            .WithIdentity(RepeatingJob.RepeatingJobKey) // 使用 JobKey
            .Build();
    }

    public override ITrigger CreateTrigger()
    {
        return TriggerBuilder.Create()
            .WithIdentity($"trigger_{Guid.NewGuid()}", "backgroundGroup")
            .ForJob(RepeatingJob.RepeatingJobKey) // 绑定触发器到特定的 Job
            .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(10)  // 每60秒重复
            .RepeatForever())  // 无限重复
            .Build();



        /// return TriggerBuilder.Create()
        ///  .WithIdentity($"trigger_{Guid.NewGuid()}", "defaultGroup")
        ///  .ForJob(NotificationJob.NotificationJobKey)
        ///  .StartAt(request.ReminderTime)  // 初次执行时间
        ///  .WithCronSchedule("0 0/5 * * * ?")  // 每5分钟执行一次
        ///  .Build();
    }
}

public class SchedulerJobFactory : BaseJobFactory
{
    public override IJobDetail CreateJob()
    {
        return JobBuilder.Create<SchedulerJob>()
            .WithIdentity(SchedulerJob.SchedulerJobKey) // 使用 JobKey
            .Build();
    }

    public override ITrigger CreateTrigger()
    {
        var malaysiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kuala_Lumpur");
        return TriggerBuilder.Create()
            .WithIdentity($"trigger_{Guid.NewGuid()}", "backgroundGroup")
            .ForJob(SchedulerJob.SchedulerJobKey) // 绑定触发器到特定的 Job
            .WithCronSchedule("30 13 * * * ?", s => s.InTimeZone(malaysiaTimeZone))
            .Build();

    }
}