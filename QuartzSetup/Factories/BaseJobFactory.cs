using Quartz;
using QuartzSetup.Jobs.JobRequest;

namespace QuartzSetup.Factories;

public class BaseJobFactory : IJobFactory
{
    public virtual IJobDetail CreateJob()
    {
        return null!;
    }

    public virtual IJobDetail CreateJob(IJobRequest request)
    {
        return null!;
    }

    public virtual ITrigger CreateTrigger()
    {
        return null!;
    }

    public virtual ITrigger CreateTrigger(IJobRequest request)
    {
        return null!;
    }
}
